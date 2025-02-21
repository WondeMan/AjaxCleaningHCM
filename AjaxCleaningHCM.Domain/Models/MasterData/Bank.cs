using AjaxCleaningHCM.Domain.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static AjaxCleaningHCM.Domain.Enums.Common;

namespace AjaxCleaningHCM.Domain.Models.MasterData
{
    public class Bank : AuditAttribute
    {
        [Key]
        public long Id { get; set; }
        [Required, Display(Name = "Bank Name")]
        public string BankName { get; set; }
        [Required, Display(Name = "Bank Code")]
        public string BankCode { get; set; }
        public MainCurrency MainCurrency { get; set; }
    }
}
