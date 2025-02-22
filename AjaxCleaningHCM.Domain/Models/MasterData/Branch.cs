using AjaxCleaningHCM.Domain.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AjaxCleaningHCM.Domain.Models.MasterData
{
    public class Branch : AuditAttribute
    {
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required, Display(Name = "Name")]
        public string Name { get; set; }
        public string Code { get; set; }
        [Required, Display(Name = "Description")]
        public string Description { get; set; }
        [Required, Display(Name = "Address")]
        public string Address { get; set; }
        [Required, Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "Physical Location")]
        public string PhysicalLocation { get; set; }
        [DataType(DataType.PhoneNumber), Phone]
        public string Phone { get; set; }
        [Required, Display(Name = "Number Of Employees")]
        public int NumberOfEmployees { get; set; } //to be updated on employee registeration
        public virtual ICollection<EmployeeBranch> EmployeeBranches { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
