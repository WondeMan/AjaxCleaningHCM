using AjaxCleaningHCM.Domain.DTO.MasterData.Request;
using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.Models.MasterData;
using System;
using System.Collections.Generic;
using System.Text;

namespace AjaxCleaningHCM.Domain.DTO.MasterData.Response
{
    public class RegionResponseDto : OperationStatusResponse
    {
        public RegionDto RegionDto { get; set; }
    }
    public class RegionsResponseDto : OperationStatusResponse
    {
        public List<RegionDto> RegionDtos { get; set; }
    }
}
