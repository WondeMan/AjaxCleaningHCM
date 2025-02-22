using AjaxCleaningHCM.Domain.DTO.MasterData.Request;
using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.Models.MasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static AjaxCleaningHCM.Domain.Enums.Common;

namespace AjaxCleaningHCM.Domain.DTO.MasterData.Response
{
    public class BankResponseDto : OperationStatusResponse
    {
        public Bank BankDto { get; set; }
    }
    public class BankResponseDtos : OperationStatusResponse
    {
        public List<Bank> BankDtos { get; set; }
    }
}
