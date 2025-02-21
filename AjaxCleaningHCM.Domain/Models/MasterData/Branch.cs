using AjaxCleaningHCM.Domain.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AjaxCleaningHCM.Domain.Models.MasterData
{
    public class Branch : AuditAttribute
    {
        [Required]
        public long Id { get; set; }
        [Required, Display(Name = "Branch Name")]
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        [Required, Display(Name = "Description")]
        public string Description { get; set; }
        [Required, Display(Name = "Address")]
        public string Address { get; set; }
        [Required, Display(Name = "City")]
        public string City { get; set; } //City table r/ship
        [Display(Name = "Physical Location")]
        public string PhysicalLocation { get; set; }
        [DataType(DataType.PhoneNumber), Phone]
        public string Phone { get; set; }
        [Required, Display(Name = "Number Of Employees")]
        public int NumberOfEmployees { get; set; } //to be updated on employee registeration
        //[Required, Display(Name = "Contact Person"),ForeignKey("ContactPerson")]
        public long? ContactPersonId { get; set; } //Employee
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string EmailAddress { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
        //public virtual Employee ContactPerson { get; set; } //Employee
    }
}
