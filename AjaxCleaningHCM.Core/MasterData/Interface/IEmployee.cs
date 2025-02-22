using AjaxCleaningHCM.Core.Helper;
using AjaxCleaningHCM.Domain.DTO.MasterData.Response;
using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.Models.MasterData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
        Task<EmployeeResponseDtos> Search(EmployeeSearchRequest EmployeeSearchRequest);

    }
}
