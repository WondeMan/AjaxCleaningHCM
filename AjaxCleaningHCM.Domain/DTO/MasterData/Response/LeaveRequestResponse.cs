using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.Models.MasterData;
using System;
using System.Collections.Generic;
using System.Text;

namespace AjaxCleaningHCM.Domain.DTO.MasterData.Response
{
    public class LeaveRequestResponseDto : OperationStatusResponse
    {
        public LeaveRequest LeaveRequestDto { get; set; }
    }
    public class LeaveRequestResponseDtos : OperationStatusResponse
    {
        public List<LeaveRequest> LeaveRequestDtos { get; set; }
    }
}
