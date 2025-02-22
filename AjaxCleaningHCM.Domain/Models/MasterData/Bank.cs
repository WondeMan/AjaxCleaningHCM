using AjaxCleaningHCM.Domain.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static AjaxCleaningHCM.Domain.Enums.Common;

namespace AjaxCleaningHCM.Domain.Models.MasterData
{
    public class Bank : AuditAttribute
    {
        [Key]
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required, Display(Name = "Bank Name")]
        public string Name { get; set; }
        [Required, Display(Name = "Bank Code")]
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
