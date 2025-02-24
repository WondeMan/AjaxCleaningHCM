//using AjaxCleaningHCM.Domain.Models.MasterData;
//using AjaxCleaningHCM.Infrastructure.Data;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using static AjaxCleaningHCM.Domain.Enums.Common;
//using System.Threading.Tasks;
//using AjaxCleaningHCM.Core.Operational.Interface;
//using AjaxCleaningHCM.Core.Helper;
//using Microsoft.AspNetCore.Http;
//using AjaxCleaningHCM.Core.MasterData.Interface;

//namespace AjaxCleaningHCM.Core.Operational.Service
//{
//    public class PayrollService : IPayrollService
//    {
//        private readonly IRepositoryBase<Payroll> _PayrollRepository;
//        private readonly IHttpContextAccessor _httpContextAccessor;
//        private readonly IEmployee _employee;
//        public PayrollService(IRepositoryBase<Payroll> PayrollRepository, IEmployee employee, IHttpContextAccessor httpContextAccessor)
//        {
//            _httpContextAccessor = httpContextAccessor;
//            _PayrollRepository = PayrollRepository;
//            _employee = employee;
//        }

//        public async Task ProcessPayroll(long employeeId)
//        {
//            var employee = await _context.Employees.FindAsync(employeeId);
//            if (employee == null)
//            {
//                throw new Exception("Employee not found.");
//            }

//            // Retrieve salary details
//            decimal basicSalary = employee.MonthlySalary;
//            decimal allowances = CalculateAllowances(employee);
//            decimal deductions = CalculateDeductions(employee);
//            decimal netSalary = basicSalary + allowances - deductions;

//            // Create payroll entry
//            var payroll = new Payroll
//            {
//                EmployeeId = employee.Id,
//                BasicSalary = basicSalary,
//                Allowances = allowances,
//                Deductions = deductions,
//                NetSalary = netSalary,
//                PaymentDate = DateTime.UtcNow,
//                PaymentStatus = PayrollStatus.Pending,
//                BankAccountNumber = employee.BankAccountNumber,
//                BankId = employee.BankId,
//                PaymentMode = PaymentMode.BankTransfer,
//                PaymentReference = GeneratePaymentReference()
//            };

//            _context.Payrolls.Add(payroll);
//            await _context.SaveChangesAsync();
//        }

//        private decimal CalculateAllowances(Employee employee)
//        {
//            // Example: Housing Allowance (10% of salary), Transport Allowance (5%)
//            decimal housingAllowance = employee.MonthlySalary * 0.10m;
//            decimal transportAllowance = employee.MonthlySalary * 0.05m;
//            return housingAllowance + transportAllowance;
//        }

//        private decimal CalculateDeductions(Employee employee)
//        {
//            // Example: Tax (10%), Pension Contribution (5%)
//            decimal tax = employee.MonthlySalary * 0.10m;
//            decimal pension = employee.MonthlySalary * 0.05m;
//            return tax + pension;
//        }

//        public async Task MarkAsPaid(long payrollId)
//        {
//            var payroll = await _context.Payrolls.FindAsync(payrollId);
//            if (payroll == null)
//            {
//                throw new Exception("Payroll record not found.");
//            }

//            payroll.PaymentStatus = PayrollStatus.Paid;
//            payroll.PaymentDate = DateTime.UtcNow;

//            await _context.SaveChangesAsync();
//        }

//        private string GeneratePaymentReference()
//        {
//            return $"PAY-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
//        }
//    }
//}
