using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.Models.MasterData;
using System;
using System.Collections.Generic;
using System.Text;

namespace AjaxCleaningHCM.Domain.DTO.MasterData.Response
{
    public class LeaveTypeResponseDto : OperationStatusResponse
    {
        public LeaveType LeaveTypeDto { get; set; }
    }
    public class LeaveTypeResponseDtos : OperationStatusResponse
    {
        public List<LeaveType> LeaveTypeDtos { get; set; }
    }
}
