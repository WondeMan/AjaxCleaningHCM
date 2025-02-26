using AjaxCleaningHCM.Domain.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AjaxCleaningHCM.Domain.Models.MasterData
{
    public class LeaveRequest : AuditAttribute
    {
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(Employee))]
        [Required(ErrorMessage = "This field is required")]
        public long EmployeeId { get; set; }
        [ForeignKey(nameof(LeaveType))]
        [Required(ErrorMessage = "This field is required")]
        public long LeaveTypeId { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public int Year { get; set; }
        public int NumberOfWorkingDay { get; set; }
        public int NumberOfHolidayDay { get; set; }
        public int TotalLeaveDay { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int AllowedDay { get; set; }
        public int RemainingDay { get; set; }
        public DateTime EmployeeStartDate { get; set; }
        public string Remark { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual LeaveType LeaveType { get; set; }
    }

}
