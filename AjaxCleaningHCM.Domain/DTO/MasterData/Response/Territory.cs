using AjaxCleaningHCM.Domain.DTO.MasterData.Request;
using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.Models.MasterData;
using System;
using System.Collections.Generic;
using System.Text;

namespace AjaxCleaningHCM.Domain.DTO.MasterData.Response
{
    public class TerritoryResponseDto : OperationStatusResponse
    {
        public TerritoryDto TerritoryDto { get; set; }
    }
    public class TerritorysResponseDto : OperationStatusResponse
    {
        public List<TerritoryDto> TerritoryDtos { get; set; }
    }
}
