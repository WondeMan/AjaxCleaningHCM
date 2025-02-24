using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static AjaxCleaningHCM.Domain.Enums.Common;
using AjaxCleaningHCM.Domain.Models.Helper;

namespace AjaxCleaningHCM.Domain.Models.MasterData
{
    public class Payroll : AuditAttribute
    {
        [Key]
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long Id { get; set; }

        [ForeignKey("Employee"), Required]
        public long EmployeeId { get; set; }
        public virtual Employee Employee { get; set; } // Navigation property to Employee

        [Required]
        [Display(Name = "Basic Salary")]
        public decimal BasicSalary { get; set; } // Base salary before deductions and allowances

        [Display(Name = "Allowances")]
        public decimal Allowances { get; set; } = 0; // Additional earnings like housing or transport

        [Display(Name = "Deductions")]
        public decimal Deductions { get; set; } = 0; // Tax, loan, pension, etc.

        [Required]
        [Display(Name = "Net Salary")]
        public decimal NetSalary { get; set; } = 0;

        [Required]
        [Display(Name = "Payment Date")]
        public DateTime PaymentDate { get; set; } // Date when salary is paid

        [Required]
        [Display(Name = "Payment Status")]
        public PayrollStatus PaymentStatus { get; set; } // Enum: Pending, Processed, Paid

        [Display(Name = "Bank Account")]
        public string BankAccountNumber { get; set; } // Employee's bank account

        [ForeignKey("Bank")]
        public long? BankId { get; set; }
        public virtual Bank Bank { get; set; } // Navigation property to Bank

        [Display(Name = "Payment Reference")]
        public string PaymentReference { get; set; } // Reference number for transaction tracking

        [Display(Name = "Payment Mode")]
        public PaymentMode PaymentMode { get; set; } // Enum: Bank Transfer, Cash, Check

        [Display(Name = "Remarks")]
        public string Remarks { get; set; } // Additional notes about payment
    }

}
