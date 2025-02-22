using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.Models.MasterData;
using System;
using System.Collections.Generic;
using System.Text;

namespace AjaxCleaningHCM.Domain.DTO.MasterData.Response
{
    public class EmployeeTerminationResponseDto : OperationStatusResponse
    {
        public EmployeeTermination EmployeeTerminationDto { get; set; }
    }
    public class EmployeeTerminationResponseDtos : OperationStatusResponse
    {
        public List<EmployeeTermination> EmployeeTerminationDtos { get; set; }
    }
}
