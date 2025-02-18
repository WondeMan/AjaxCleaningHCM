using AjaxCleaningHCM.Domain.DTO.MasterData.Request;
using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.Models.MasterData;
using System;
using System.Collections.Generic;
using System.Text;

namespace AjaxCleaningHCM.Domain.DTO.MasterData.Response
{
    public class ShopResponseDto : OperationStatusResponse
    {
        public ShopDto ShopDto { get; set; }
    }
    public class ShopsResponseDto : OperationStatusResponse
    {
        public List<ShopDto> ShopDtos { get; set; }
    }
}
