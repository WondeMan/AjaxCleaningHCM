﻿using AjaxCleaningHCM.Core.Helper;
using AjaxCleaningHCM.Domain.DTO.MasterData.Response;
using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.Models.MasterData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AjaxCleaningHCM.Core.MasterData.Interface
{
    public interface IBranch
    {
        long GetLastInserted();
        Task<BranchResponseDtos> GetAllAsync();
        Task<BranchResponseDto> GetByIdAsync(long id);
        Task<BranchResponseDto> CreateAsync(Branch request);
        Task<BranchResponseDto> UpdateAsync(Branch request);
        Task<OperationStatusResponse> DeleteAsync(long id);
        List<long> GetBranchId();
    }
}
