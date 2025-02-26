using AjaxCleaningHCM.Domain.Models.MasterData;
using AjaxCleaningHCM.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;
using static AjaxCleaningHCM.Domain.Enums.Common;
using System.Threading.Tasks;
using AjaxCleaningHCM.Core.Operational.Interface;
using AjaxCleaningHCM.Core.Helper;
using Microsoft.AspNetCore.Http;
using AjaxCleaningHCM.Core.MasterData.Interface;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;
using AjaxCleaningHCM.Domain.DTO.MasterData.Response;
using AjaxCleaningHCM.Domain.Models.Helper;
using Microsoft.EntityFrameworkCore;

namespace AjaxCleaningHCM.Core.Operational.Service
{
    public class PayrollService : IPayrollService
    {
        private readonly IRepositoryBase<Payroll> _PayrollRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmployee _employee;

        public PayrollService(IRepositoryBase<Payroll> PayrollRepository, IEmployee employee, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _PayrollRepository = PayrollRepository;
            _employee = employee;
        }

        // ✅ Process Payroll for ALL Employees
        public async Task<OperationStatusResponse> ProcessPayrollForBulkEmployees(List<long> employeeIds, int year=0, int month=0)
        {
            try
            {
                var employees = await _employee.GetBulkEmployeeByIdAsync(employeeIds);
                if (employees == null || !employees.EmployeeDtos.Any())
                {
                    throw new Exception("No employees found.");
                }
                List<Payroll> payrollList = new List<Payroll>();
                // ✅ Check if payroll exists for the given month
                if (year == 0)
                {
                    year = DateTime.Now.Year;
                }
                if (month == 0)
                {
                    month = DateTime.Now.Month;
                }
                var existedPayroll = _PayrollRepository.Where(p =>
                    p.PaymentDate.Year == year &&
                    p.PaymentDate.Month == month &&
                    p.RecordStatus==RecordStatus.Active);
                foreach (var employee in employees.EmployeeDtos)
                {
                    try
                    {


                        if (existedPayroll.Any(a => a.Employee.Id == employee.Id))
                        {
                            Console.WriteLine($"Skipping payroll for {employee.FullName} ({month}/{year} already processed).");
                            continue;
                        }

                        var payroll = GeneratePayroll(employee, year, month);
                        payrollList.Add(payroll);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing payroll for {employee.FullName}: {ex.Message}");
                    }
                }

                if (payrollList.Any())
                {
                    if (await _PayrollRepository.AddRangeAsync(payrollList))
                        return new OperationStatusResponse { Message = "Operation Successfully Completed", Status = OperationStatus.SUCCESS };
                    else
                        return new PayrollResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };

                }
                return new PayrollResponseDto { Message = "There is no new payroll", Status = OperationStatus.SUCCESS };

            }
            catch (Exception ex)
            {

                return new PayrollResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
                ;
            }

        }


        // ✅ Process Payroll for a Single Employee
        public async Task<OperationStatusResponse> ProcessPayroll(long employeeId, int year=0, int month=0)
        {
            var employee = await _employee.GetByIdAsync(employeeId);
            if (employee == null)
            {
                throw new Exception("Employee not found.");
            }
            if (year == 0)
            {
                year = DateTime.Now.Year;
            }
            if (month == 0)
            {
                month = DateTime.Now.Month;
            }
            // ✅ Check if payroll already exists for the given month
            bool payrollExists = _PayrollRepository.Where(p =>
                p.EmployeeId == employee.EmployeeDto.Id &&
                p.PaymentDate.Year == year &&
                p.PaymentDate.Month == month).Any();

            if (payrollExists)
            {
                throw new Exception($"Payroll for {employee.EmployeeDto.FullName} has already been processed for {month}/{year}.");
            }

            // ✅ Generate and Save Payroll
            var payroll = GeneratePayroll(employee.EmployeeDto, year, month);
            if(_PayrollRepository.Add(payroll)) 
                return new OperationStatusResponse { Message = "Operation Successfully Completed", Status = OperationStatus.SUCCESS };
            else
                return new PayrollResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };

        }

        private Payroll GeneratePayroll(Employee employee, int year, int month)
        {
            decimal basicSalary = employee.MonthlySalary;
            decimal allowances = CalculateAllowances(employee);
            decimal deductions = CalculateDeductions(employee);
            decimal netSalary = basicSalary + allowances - deductions;

            return new Payroll
            {
                EmployeeId = employee.Id,
                BasicSalary = basicSalary,
                Allowances = allowances,
                Deductions = deductions,
                NetSalary = netSalary,
                PaymentDate = new DateTime(year, month, DateTime.UtcNow.Day), // 🔥 Use given year & month
                PaymentStatus = PayrollStatus.Pending,
                BankAccountNumber = employee.BankAccountNumber,
                BankId = employee.BankId,
                PaymentMode = PaymentMode.BankTransfer,
                PaymentReference = GeneratePaymentReference(),
                RecordStatus = RecordStatus.Active
            };
        }
        private decimal CalculateAllowances(Employee employee)



        {
            decimal housingAllowance = employee.MonthlySalary * 0.10m;
            decimal transportAllowance = employee.MonthlySalary * 0.05m;
            return housingAllowance + transportAllowance;
        }

        private decimal CalculateDeductions(Employee employee)
        {
            decimal tax = GetTax(employee.MonthlySalary);
            decimal pension = employee.MonthlySalary * 0.05m;
            return tax + pension;
        }

        public decimal GetTax(decimal income)
        {
            decimal[] taxThresholds = { 650, 1650, 3200, 5250, 7800, 10900, decimal.MaxValue };
            decimal[] taxRates = { 0.00m, 0.10m, 0.15m, 0.20m, 0.25m, 0.30m, 0.35m };
            decimal[] deduct = { 0.00m, 60.00m, 142.50m, 302.50m, 565.00m, 955.00m, 1500.00m };
            decimal tax = 0;

            for (int i = 0; i < taxThresholds.Length; i++)
            {
                if (income <= taxThresholds[i])
                {
                    tax = income * taxRates[i] - deduct[i];
                    break;
                }
                else
                {
                    tax = taxThresholds[i] * taxRates[i] - deduct[i];
                    income -= tax;
                }
            }
            return tax;
        }

        private string GeneratePaymentReference()
        {
            return $"PAY-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }

        public async Task<PayrollResponseDtos> GetAllAsync()
        {
             try
            {
                var result = new PayrollResponseDtos();
                var PayrollResponses = await _PayrollRepository.Where(x => x.RecordStatus == RecordStatus.Active && x.PaymentDate.Month==DateTime.Now.Month).Include(a=>a.Employee).ToListAsync();
                var PayrollDTOs = new List<PayrollResponseDtos>();
                result.Status = OperationStatus.SUCCESS;
                result.Message = "Operation Successfully Completed";
                result.PayrollDtos = PayrollResponses.ToList();
                return result;
            }
            catch (Exception ex)
            {
                return new PayrollResponseDtos { PayrollDtos = new List<Payroll>(), Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<PayrollResponseDtos> Search(int year, int month)
        {
            try
            {
                var result = new PayrollResponseDtos();
                var PayrollResponses = await _PayrollRepository.Where(x => x.RecordStatus == RecordStatus.Active && x.PaymentDate.Month == month && x.PaymentDate.Year==year).Include(a => a.Employee).ToListAsync();
                var PayrollDTOs = new List<PayrollResponseDtos>();
                result.Status = OperationStatus.SUCCESS;
                result.Message = "Operation Successfully Completed";
                result.PayrollDtos = PayrollResponses.ToList();
                return result;
            }
            catch (Exception ex)
            {
                return new PayrollResponseDtos { PayrollDtos = new List<Payroll>(), Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public Task<PayrollResponseDtos> GetUnPaidAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PayrollResponseDto> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationStatusResponse> DeleteAsync(long id)
        {
            try
            {
                var Bank = await _PayrollRepository.FirstOrDefaultAsync(u => u.Id == id);

                if (Bank == null)
                    return new OperationStatusResponse { Message = "Record Does Not Exist", Status = OperationStatus.ERROR };

                Bank.RecordStatus = RecordStatus.Deleted;
                if (await _PayrollRepository.UpdateAsync(Bank))
                    return new OperationStatusResponse { Message = "Operation Successfully Completed", Status = OperationStatus.SUCCESS };
                else
                    return new BankResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
            catch (Exception ex)
            {
                return new BankResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
    }
}
