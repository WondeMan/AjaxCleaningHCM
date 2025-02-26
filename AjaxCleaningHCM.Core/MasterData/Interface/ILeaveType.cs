﻿using AjaxCleaningHCM.Domain.DTO.MasterData.Response;
using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.Models.MasterData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AjaxCleaningHCM.Core.MasterData.Interface
{
    public interface ILeaveType
    {
        Task<LeaveTypeResponseDtos> GetAllAsync();
        Task<LeaveTypeResponseDto> GetByIdAsync(long id);
        Task<LeaveTypeResponseDto> CreateAsync(LeaveType request);
        Task<LeaveTypeResponseDto> UpdateAsync(LeaveType request);
        Task<OperationStatusResponse> DeleteAsync(long id);
    }
}
