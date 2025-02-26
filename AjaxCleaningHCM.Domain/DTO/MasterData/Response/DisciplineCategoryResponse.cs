using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.Models.MasterData;
using System;
using System.Collections.Generic;
using System.Text;

namespace AjaxCleaningHCM.Domain.DTO.MasterData.Response
{
    public class DisciplineCategoryResponseDto : OperationStatusResponse
    {
        public DisciplineCategory DisciplineCategoryDto { get; set; }
    }
    public class DisciplineCategoryResponseDtos : OperationStatusResponse
    {
        public List<DisciplineCategory> DisciplineCategoryDtos { get; set; }
    }
}
