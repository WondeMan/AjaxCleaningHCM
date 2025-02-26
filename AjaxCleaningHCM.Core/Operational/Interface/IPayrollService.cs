using AjaxCleaningHCM.Domain.DTO.MasterData.Response;
using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.Models.MasterData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AjaxCleaningHCM.Core.Operational.Interface
{
    public interface IPayrollService
    {
        Task<OperationStatusResponse> ProcessPayrollForBulkEmployees(List<long> employeeId, int year = 0, int month = 0);
        Task<OperationStatusResponse> ProcessPayroll(long employeeId, int year = 0, int month = 0);


        Task<PayrollResponseDtos> GetAllAsync();
        Task<PayrollResponseDtos> Search(int year, int month);
        Task<PayrollResponseDtos> GetUnPaidAsync();
        Task<PayrollResponseDto> GetByIdAsync(long id);
        Task<OperationStatusResponse> DeleteAsync(long id);
    }
}
