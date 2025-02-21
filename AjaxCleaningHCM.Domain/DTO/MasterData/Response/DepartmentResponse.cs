using AjaxCleaningHCM.Domain.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static AjaxCleaningHCM.Domain.Enums.Common;

namespace AjaxCleaningHCM.Domain.DTO.MasterData.Response
{

    public class DepartmentResponseDto : OperationStatusResponse
    {
        public DepartmentDto DepartmentDto { get; set; }
    }
    public class DepartmentsResponseDto : OperationStatusResponse
    {
        public List<DepartmentDto> DepartmentDtos { get; set; }
    }
    public class DepartmentDto : OperationStatusResponse
    {
        public long Id { get; set; }
        [Required, Display(Name = "Department Name")]
        public string DepartmentName { get; set; }
        [Required, Display(Name = "Department Description")]
        public string DepartmentDescription { get; set; }
    }
}
