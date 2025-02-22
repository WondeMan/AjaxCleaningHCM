using AjaxCleaningHCM.Domain.Enums;
using AjaxCleaningHCM.Domain.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static AjaxCleaningHCM.Domain.Enums.Common;

namespace AjaxCleaningHCM.Domain.Models.MasterData
{
    public class EmployeeTermination : AuditAttribute
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(Employee))]
        [Required(ErrorMessage = "Employee is required"), Display(Name = "Employee")]
        public long EmployeeId { get; set; }
        [Required(ErrorMessage = "Date is required")]
        public DateTime TerminationDate { get; set; }
        public TerminationReason TerminationReason { get; set; }
        public string Remark { get; set; }
        public string Letter { get; set; }
        public LetterType LetterType { get; set; }
        public EmployeeStatus EmployeeStatus { get; set; } = EmployeeStatus.Terminated;
        public virtual Employee Employee { get; set; }
    }
}
