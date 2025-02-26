using AjaxCleaningHCM.Core.Helper;
using AjaxCleaningHCM.Domain.DTO.MasterData.Response;
using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.Models.MasterData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static AjaxCleaningHCM.Domain.Enums.Common;

namespace AjaxCleaningHCM.Core.MasterData.Interface
{
    public interface IEmployee
    {
        long GetLastInserted();
        Task<EmployeeResponseDtos> GetAllAsync();
        Task<EmployeeResponseDto> GetByIdAsync(long id);
        Task<EmployeeResponseDto> GetByUserNameAsync(string userName);
        Task<EmployeeResponseDto> CreateAsync(Employee request);
        Task<EmployeeResponseDto> UpdateAsync(Employee request);
        Task<OperationStatusResponse> DeleteAsync(long id);
        Employee GetEmployee();
        OperationStatus Remove(long id);
        OperationStatus ActivateEmployee(long id);
        Task<EmployeeResponseDtos> Search(EmployeeSearchRequest EmployeeSearchRequest);
        Task<EmployeeResponseDto> UpdateEmployeeStatusAsync(Employee request);
        Task<EmployeeResponseDtos> GetBulkEmployeeByIdAsync(List<long> employeeIds);
    }
}
