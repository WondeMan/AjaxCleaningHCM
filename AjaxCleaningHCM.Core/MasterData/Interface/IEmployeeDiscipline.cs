using AjaxCleaningHCM.Domain.DTO.MasterData.Response;
using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.Models.MasterData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AjaxCleaningHCM.Core.MasterData.Interface
{
    public interface IEmployeeDiscipline
    {
        Task<EmployeeDisciplineResponseDtos> GetAllAsync();
        Task<EmployeeDisciplineResponseDto> GetByIdAsync(long id);
        Task<EmployeeDisciplineResponseDto> CreateAsync(EmployeeDiscipline request);
        Task<EmployeeDisciplineResponseDto> UpdateAsync(EmployeeDiscipline request);
        Task<OperationStatusResponse> DeleteAsync(long id);
    }
}
