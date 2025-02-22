using AjaxCleaningHCM.Core.Helper;
using AjaxCleaningHCM.Domain.DTO.MasterData.Request;
using AjaxCleaningHCM.Domain.DTO.MasterData.Response;
using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.Models.MasterData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AjaxCleaningHCM.Core.MasterData.Interface
{
    public interface IBank
    {
        Task<BankResponseDtos> GetAllAsync();
        Task<BankResponseDto> GetByIdAsync(long id);
        Task<BankResponseDto> CreateAsync(Bank request);
        Task<BankResponseDto> UpdateAsync(Bank request);
        Task<OperationStatusResponse> DeleteAsync(long id);
    }
}
