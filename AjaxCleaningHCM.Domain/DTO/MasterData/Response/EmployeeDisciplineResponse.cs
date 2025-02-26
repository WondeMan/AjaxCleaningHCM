using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.Models.MasterData;
using System;
using System.Collections.Generic;
using System.Text;

namespace AjaxCleaningHCM.Domain.DTO.MasterData.Response
{
    public class EmployeeDisciplineResponseDto : OperationStatusResponse
    {
        public EmployeeDiscipline EmployeeDisciplineDto { get; set; }
    }
    public class EmployeeDisciplineResponseDtos : OperationStatusResponse
    {
        public List<EmployeeDiscipline> EmployeeDisciplineDtos { get; set; }
    }
}
