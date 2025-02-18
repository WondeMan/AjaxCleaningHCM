using AjaxCleaningHCM.Domain.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static AjaxCleaningHCM.Domain.Enums.Common;

namespace AjaxCleaningHCM.Domain.DTO.MasterData.Response
{
    public class BranchResponseDto : OperationStatusResponse
    {
        public BranchDto BranchDto { get; set; }
    }
    public class BranchsResponseDto : OperationStatusResponse
    {
        public List<BranchDto> BranchDtos { get; set; }
    }
    public class BranchDto : OperationStatusResponse
    {
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

    }
}
