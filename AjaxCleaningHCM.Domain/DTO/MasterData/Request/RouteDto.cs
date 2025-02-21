using AjaxCleaningHCM.Domain.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AjaxCleaningHCM.Domain.Models.MasterData
{
    [Table("Route")]
    public class RouteDto : AuditAttribute
    {
        public int Id { get; set; }
        [Required]
        public string RouteName { get; set; }
        public long Distance { get; set; }
        public long Latitude { get; set; }
        public long Longitude { get; set; }
        public long TerritoryId { get; set; }
    }
}
