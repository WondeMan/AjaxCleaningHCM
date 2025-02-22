using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AjaxCleaningHCM.Domain.Models.MasterData
{
    public class EmployeeBranch
    {
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey("Employee"), Required]
        public long EmployeeId { get; set; }
        [ForeignKey("Branch"), Required]
        public long BranchId { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Branch Branch { get; set; }
    }
}
