using AjaxCleaningHCM.Domain.DTO.MasterData.Response;
using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.Models.MasterData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AjaxCleaningHCM.Core.MasterData.Interface
{

    public interface IEmployeeTermination
    {
        Task<EmployeeTerminationResponseDtos> GetAllAsync();
        Task<EmployeeTerminationResponseDto> GetByIdAsync(long id);
        Task<EmployeeTerminationResponseDto> CreateAsync(EmployeeTermination request);
        Task<EmployeeTerminationResponseDto> UpdateAsync(EmployeeTermination request);
        Task<OperationStatusResponse> DeleteAsync(long id);
        Task<EmployeeTerminationResponseDtos> Search(SearchRequest request);
        Task<EmployeeTerminationResponseDto> GetByEmplyeeId(long id);
    }
}
