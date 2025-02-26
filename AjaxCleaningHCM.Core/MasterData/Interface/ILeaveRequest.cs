using AjaxCleaningHCM.Domain.DTO.MasterData.Response;
using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.Models.MasterData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AjaxCleaningHCM.Core.MasterData.Interface
{
    public interface ILeaveRequest
    {
        Task<LeaveRequestResponseDtos> GetAllAsync();
        Task<LeaveRequestResponseDto> GetByIdAsync(long id);
        Task<LeaveRequestResponseDto> CreateAsync(LeaveRequest request);
        Task<LeaveRequestResponseDto> UpdateAsync(LeaveRequest request);
        Task<OperationStatusResponse> DeleteAsync(long id);
        Task<LeaveRequestResponseDtos> FiltereavBalanceByDate(DateTime startDate, DateTime endDate, string employeeId, int? year);

    }
}
