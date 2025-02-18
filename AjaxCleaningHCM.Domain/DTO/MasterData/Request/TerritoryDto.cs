using AjaxCleaningHCM.Domain.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Data;

namespace AjaxCleaningHCM.Domain.Models.MasterData
{
    public class TerritoryDto : AuditAttribute
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Code { get; set; }
        public long Population { get; set; }
        public long RegionId { get; set; }
    }
}
