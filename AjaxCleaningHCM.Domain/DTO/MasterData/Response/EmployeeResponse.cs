using AjaxCleaningHCM.Domain.Models;
using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.Models.MasterData;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static AjaxCleaningHCM.Domain.Enums.Common;

namespace AjaxCleaningHCM.Domain.DTO.MasterData.Response
{
    public class EmployeeResponseDto : OperationStatusResponse
    {
        public Employee EmployeeDto { get; set; }
    }
    public class EmployeeResponseDtos : OperationStatusResponse
    {
        public List<Employee> EmployeeDtos { get; set; }
    }
}
