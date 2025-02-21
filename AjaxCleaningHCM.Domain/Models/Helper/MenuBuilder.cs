using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AjaxCleaningHCM.Domain.Models.Helper
{
    public class MenuBuilder : AuditAttribute
    {
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long Id { get; set; }
        [Required]
        public string LinkText { get; set; }
        [Required]
        public string LinkUrl { get; set; }
        public string Icon { get; set; }
        public long? ParentId { get; set; }
        public int? Order { get; set; }

    }
}
