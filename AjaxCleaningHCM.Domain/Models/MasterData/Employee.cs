using AjaxCleaningHCM.Domain.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static AjaxCleaningHCM.Domain.Enums.Common;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

namespace AjaxCleaningHCM.Domain.Models.MasterData
{
    public class Employee : AuditAttribute
    {
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required, Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string EmployeeId { get; set; }

        [Required, Display(Name = "Date Of Birth")]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [DataType(DataType.PhoneNumber), Required, Phone, Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Emergency Contact"), Required]
        public string EmergencyContactFullName { get; set; }
        [Phone, DataType(DataType.PhoneNumber), Display(Name = "Emergency Contact"), Required]
        public string EmergencyContactPhoneNumber { get; set; }
        [Display(Name = "Full Name")]
        public string FullName
        {
            get { return FirstName + " " + MiddleName + " " + LastName; }
        }
        [Required]
        public EducationStatus EducationStatus { get; set; }
        public string FieldOfStudy { get; set; }
        public string Institution { get; set; }
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
        //public string Photo { get; set; }
        [Display(Name = "Employment Start Date"), Required]
        public DateTime EmploymentStartDate { get; set; }
        [Display(Name = "Employment End Date")]
        public DateTime? EmploymentEndDate { get; set; } = DateTime.MaxValue;
        [Display(Name = "Job Title"), Required]
        public string JobTitle { get; set; }
        [Display(Name = "Job Position"), Required]
        public string JobPosition { get; set; }
        [ForeignKey("Branch"), Required]
        public long BranchId { get; set; }
        [Display(Name = "Monthly Salary")]
        public decimal MonthlySalary { get; set; }
        [Display(Name = "Bank Account")]
        public string BankAccountNumber { get; set; }
        [ForeignKey("Bank"), Display(Name = "Bank")]
        public long? BankId { get; set; }
        public decimal MaximumCredit { get; set; }
        public int NumberOfVacation { get; set; }
        [NotMapped]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".pdf", ".doc", ".docx", ".gif" })]
        [Required]
        [DataType(DataType.Upload)]
        public IFormFile EmpoloyePhotoImg { get; set; }
        [NotMapped]
        [Required]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".pdf", ".doc", ".docx", ".gif" })]
        [DataType(DataType.Upload)]
        public IFormFile EmployeeKebeleIDImg { get; set; }
        [NotMapped]
        [Required]
        [DataType(DataType.Upload)]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".pdf", ".doc", ".docx", ".gif" })]
        public IFormFile GurrentKebeleIDImg { get; set; }
        [NotMapped]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".pdf", ".doc", ".docx", ".gif" })]
        public IFormFile GuaranteeDocumentImg { get; set; }
        [NotMapped]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".pdf", ".doc", ".docx", ".gif" })]
        public IFormFile CirtificationImg { get; set; }
        public string EmpoloyePhotoPath { get; set; }
        public string EmployeeKebeleIDPath { get; set; }
        public string GurrentKebeleIDPath { get; set; }
        public string GurrentDocumentPath { get; set; }
        public string CirtificationPath { get; set; }
        [NotMapped]
        [Display(Name = "Image")]
        [DataType(DataType.Upload)]
        public IFormFile Photo { get; set; }
        public virtual Bank Bank { get; set; }
        public virtual ICollection<EmployeeBranch> EmployeeBranches { get; set; }
        [NotMapped]
        public virtual List<long> BrancheIds { get; set; }
    }
    public class EmployeeFile
    {
        public string Diroctory { get; set; }
        public IFormFile Attachement { get; set; }
    }
    public class EmployeeSearchRequest
    {
        public SearchBygroup SearchBygroup { get; set; }
        public Gender Gender { get; set; }
        public EducationStatus EducationStatus { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }
        public int Experience { get; set; }
    }
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;
        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"This extension is not allowed!";
        }
    }
}
