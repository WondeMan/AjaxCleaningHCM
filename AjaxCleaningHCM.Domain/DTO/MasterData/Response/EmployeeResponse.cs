using AjaxCleaningHCM.Domain.Models;
using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.Models.MasterData;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static AjaxCleaningHCM.Domain.Enums.Common;

namespace AjaxCleaningHCM.Domain.DTO.MasterData.Response
{
    public class EmployeeResponseDto : OperationStatusResponse
    {
        public EmployeeDto EmployeeDto { get; set; }
    }
    public class EmployeesResponseDto : OperationStatusResponse
    {
        public List<EmployeeDto> EmployeeDtos { get; set; }
    }
    public class EmployeeDto : OperationStatusResponse
    {
        public long Id { get; set; }
        [Display(Name = "Employee Id"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string EmployeeId { get; set; }//auto generated but length, start char need to be set
        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required, Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required, Display(Name = "Date Of Birth")]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public Gender Gender { get; set; }
        public string Address { get; set; }
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
        public EmploymentType EmploymentType { get; set; }
        public string FieldOfStudy { get; set; }
        public decimal MaximumCredit { get; set; }
        public int NumberOfVacation { get; set; }
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
        [ForeignKey("Shift"), Display(Name = "Shift")]
        public long ShiftId { get; set; }
        [Display(Name = "Weekly Pay")]
        public decimal WeeklyPay { get; set; }
        [Display(Name = "Monthly Salary")]
        public decimal MonthlySalary { get; set; }
        [Display(Name = "Bank Account")]
        public string BankAccountNumber { get; set; }
        [ForeignKey("Bank"), Display(Name = "Bank")]
        public long? BankId { get; set; }
        [ForeignKey("Department"), Display(Name = "Department")]
        public long? DepartmentId { get; set; }
        [NotMapped]
        public string DocumentPath { get; set; }
        [NotMapped]
        public IFormFile DocumentImg { get; set; }
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
        [NotMapped]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".pdf", ".doc", ".docx", ".gif" })]
        public IFormFile EmploymentLetterImg { get; set; }
        public string EmploymentLetterPath { get; set; }
        public string EmpoloyePhotoPath { get; set; }
        public string EmployeeKebeleIDPath { get; set; }
        public string GurrentKebeleIDPath { get; set; }
        public string GurrentDocumentPath { get; set; }
        public string CirtificationPath { get; set; }
        [NotMapped]
        [Display(Name = "Image")]
        [DataType(DataType.Upload)]
        public IFormFile Photo { get; set; }
    }
}
