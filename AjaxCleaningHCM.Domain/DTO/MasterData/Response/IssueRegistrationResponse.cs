using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.Models.MasterData;
using System;
using System.Collections.Generic;
using System.Text;

namespace AjaxCleaningHCM.Domain.DTO.MasterData.Response
{
    public class IssueRegistrationResponseDto : OperationStatusResponse
    {
        public IssueRegistration IssueRegistrationDto { get; set; }
    }
    public class IssueRegistrationResponseDtos : OperationStatusResponse
    {
        public List<IssueRegistration> IssueRegistrationDtos { get; set; }
    }
}
