using AjaxCleaningHCM.Domain.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AjaxCleaningHCM.Domain.Models.MasterData
{
    public class DisciplineCategory : AuditAttribute
    {
        [Key]
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required, Display(Name = "Discipline Category Name")]
        public string Name { get; set; }
        [Required, Display(Name = "Discipline Category Code")]
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
