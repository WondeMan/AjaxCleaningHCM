
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using AjaxCleaningHCM.Domain.Models.Helper;

namespace AjaxCleaningHCM.Domain.Models.MasterData
{
    [Table("Product")]
    public class Product : AuditAttribute
    {
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string Description { get; set; }

    }
}
