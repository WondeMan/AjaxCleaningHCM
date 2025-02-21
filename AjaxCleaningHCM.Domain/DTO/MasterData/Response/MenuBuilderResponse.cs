using AjaxCleaningHCM.Domain.Models.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace AjaxCleaningHCM.Domain.DTO.MasterData.Response
{
    public class MenuBuilderResponseDto : OperationStatusResponse
    {
        public MenuBuilder MenuBuilderDto { get; set; }
    }
    public class MenuBuilderResponseDtos : OperationStatusResponse
    {
        public List<MenuBuilder> MenuBuilderDtos { get; set; }
    }
}
