using Microsoft.AspNetCore.Identity;
using AjaxCleaningHCM.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static AjaxCleaningHCM.Domain.Enums.Common;

namespace AjaxCleaningHCM.Core.UserManagment.Identity
{
    /// <summary>
    /// Privilege base model
    /// </summary>
    public class Privilege
    {
        public string Id { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } = DateTime.MaxValue;
        public string TimeZoneInfo { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime RegisteredDate { get; set; }
        public string RegisteredBy { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string UpdatedBy { get; set; }
        public RecordStatus RecordStatus { get; set; }
    }

    /// <summary>
    /// Role privilege base model
    /// </summary>
    public class RolePrivilege
    {
        public string RoleId { get; set; }
        public string PrivilegeId { get; set; }
        public virtual Privilege Privilage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } = DateTime.MaxValue;
        public string TimeZoneInfo { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime RegisteredDate { get; set; }
        public string RegisteredBy { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string UpdatedBy { get; set; }
        public RecordStatus RecordStatus { get; set; }
    }

    /// <summary>
    /// Role base model (inherits Identity Role model
    /// </summary>
    public class Role : IdentityRole<string>
    {
        public Role() : base() { }
        public Role(string name, string _description)
            : base(name)
        {
            Description = _description;
            RoleName=name;
        }
        public virtual string Description { get; set; }
        public virtual string RoleName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } = DateTime.MaxValue;
        public string TimeZoneInfo { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime RegisteredDate { get; set; }
        public string RegisteredBy { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string UpdatedBy { get; set; }
        public RecordStatus RecordStatus { get; set; }
        public virtual ICollection<RolePrivilege> RolePrivileges { get; set; }
    }

    /// <summary>
    /// User role base model (inherits Identity User Role model)
    /// </summary>
    public class UserRole : IdentityUserRole<string>
    {
        public UserRole()
            : base()
        { }
        public virtual Role Role { get; set; }
    }

    /// <summary>
    /// User base model (inherits Identity User model)
    /// </summary>
    public class User : IdentityUser
    {
        public User()
        {
            FirstLogin = true;
        }

        //[ForeignKey("EmployeeDetails")]
        //[Display(Name = "EmployeeDetails")]
        //public long EmployeeDetailsId { get; set; }

        public bool FirstLogin { get; set; }
        public bool LockedOut { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } = DateTime.MaxValue;
        public string TimeZoneInfo { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime RegisteredDate { get; set; }
        public string RegisteredBy { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string UpdatedBy { get; set; }
        public RecordStatus RecordStatus { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

        [Required, Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required, Display(Name = "Middle name")]
        public string MiddleName { get; set; }

        [Required, Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "UserName is required.")]
        public override string UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public override string Email { get; set; }
        [NotMapped]
        public string FullName { get { return FirstName + " " + MiddleName + " " + LastName; } }
    }
    public class UserRolePrivilege
    {
        public List<RolePrivilege> RolePrivileges { get; set; }
        public List<Privilege> Privileges { get; set; }
        public List<UserRole> UserRoles { get; set; }
    }
}