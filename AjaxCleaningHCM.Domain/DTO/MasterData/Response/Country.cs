using AjaxCleaningHCM.Domain.DTO.MasterData.Request;
using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.Models.MasterData;
using System;
using System.Collections.Generic;
using System.Text;

namespace AjaxCleaningHCM.Domain.DTO.MasterData.Response
{
    public class CountryResponseDto : OperationStatusResponse
    {
        public CountryDto CountryDto { get; set; }
    }
    public class CountrysResponseDto : OperationStatusResponse
    {
        public List<CountryDto> CountryDtos { get; set; }
    }
}
