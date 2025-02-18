using AjaxCleaningHCM.Domain.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AjaxCleaningHCM.Domain.Models.MasterData
{
    public class Department : AuditAttribute
    {
        [Key]
        public long Id { get; set; }
        [Required, Display(Name = "Department Name")]
        public string DepartmentName { get; set; }
        [Required, Display(Name = "Department Description")]
        public string DepartmentDescription { get; set; }
    }
}
