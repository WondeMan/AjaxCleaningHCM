using AjaxCleaningHCM.Domain.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static AjaxCleaningHCM.Domain.Enums.Common;

namespace AjaxCleaningHCM.Domain.DTO.MasterData.Response
{
    public class EmployeeHistoryResponseDto : OperationStatusResponse
    {
        public EmployeeHistoryDto EmployeeHistoryDto { get; set; }
    }
    public class EmployeeHistorysResponseDto : OperationStatusResponse
    {
        public List<EmployeeHistoryDto> EmployeeHistoryDtos { get; set; }
    }
    public class EmployeeHistoryDto : OperationStatusResponse
    {
        public long Id { get; set; }
        [ForeignKey("Employee")]
        public long EmployeeId { get; set; }
        public string FullName { get; set; }
        public EducationStatus EducationStatus { get; set; }
        public EmploymentType EmploymentType { get; set; }
        public string FieldOfStudy { get; set; }
        public decimal MaximumCredit { get; set; }
        public int NumberOfVacation { get; set; }
        public string Institution { get; set; }
        public string JobTitle { get; set; }
        public string JobPosition { get; set; }
        public string Branch { get; set; }
        public string Shift { get; set; }
        public decimal WeeklyPay { get; set; }
        public decimal MonthlySalary { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankName { get; set; }
        public string Department { get; set; }
    }
}
