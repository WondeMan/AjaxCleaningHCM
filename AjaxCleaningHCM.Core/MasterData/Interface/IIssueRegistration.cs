using AjaxCleaningHCM.Domain.DTO.MasterData.Response;
using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.Models.MasterData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AjaxCleaningHCM.Core.MasterData.Interface
{
    public interface IIssueRegistration
    {
        Task<IssueRegistrationResponseDtos> GetAllAsync();
        Task<IssueRegistrationResponseDto> GetByIdAsync(long id);
        Task<IssueRegistrationResponseDto> CreateAsync(IssueRegistration request);
        Task<IssueRegistrationResponseDto> UpdateAsync(IssueRegistration request);
        Task<OperationStatusResponse> DeleteAsync(long id);
        Task<IssueRegistrationResponseDto> IssueProcessing(IssueRegistration request);
    }

}
