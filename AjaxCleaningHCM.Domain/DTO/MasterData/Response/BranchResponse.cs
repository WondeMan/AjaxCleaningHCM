using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.Models.MasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static AjaxCleaningHCM.Domain.Enums.Common;

namespace AjaxCleaningHCM.Domain.DTO.MasterData.Response
{
    public class BranchResponseDto : OperationStatusResponse
    {
        public Branch BranchDto { get; set; }
    }
    public class BranchResponseDtos : OperationStatusResponse
    {
        public List<Branch> BranchDtos { get; set; }
    }
}
