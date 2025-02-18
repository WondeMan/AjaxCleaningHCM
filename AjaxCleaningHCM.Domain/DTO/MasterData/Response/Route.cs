using AjaxCleaningHCM.Domain.DTO.MasterData.Request;
using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.Models.MasterData;
using System;
using System.Collections.Generic;
using System.Text;

namespace AjaxCleaningHCM.Domain.DTO.MasterData.Response
{
 
    public class RouteResponseDto : OperationStatusResponse
    {
        public RouteDto RouteDto { get; set; }
    }
    public class RoutesResponseDto : OperationStatusResponse
    {
        public List<RouteDto> RouteDtos { get; set; }
    }
}
