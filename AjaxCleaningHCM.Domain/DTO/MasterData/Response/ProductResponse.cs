using AjaxCleaningHCM.Domain.DTO.MasterData.Request;
using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.Models.MasterData;
using System;
using System.Collections.Generic;
using System.Text;

namespace AjaxCleaningHCM.Domain.DTO.MasterData.Response
{
    public class ProductResponseDto : OperationStatusResponse
    {
        public ProductDto ProductDto { get; set; }
    }
    public class ProductsResponseDto : OperationStatusResponse
    {
        public List<ProductDto> ProductDtos { get; set; }
    }
}
