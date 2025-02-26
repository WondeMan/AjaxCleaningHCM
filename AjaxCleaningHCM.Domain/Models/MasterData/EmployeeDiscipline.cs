using AjaxCleaningHCM.Core.UserManagment.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using AjaxCleaningHCM.Domain.Models.Helper;

namespace AjaxCleaningHCM.Domain.Models.MasterData
{
    public class EmployeeDiscipline : AuditAttribute
    {
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey("Employee")]
        public long EmployeeId { get; set; }
        [ForeignKey("DisciplineCategory")]
        public long DisciplineCategoryId { get; set; } 
        public string Description { get; set; }
        public string ActionTaken { get; set; }
        public DateTime ActionTakenDate { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual DisciplineCategory DisciplineCategory { get; set; }
    }
}
