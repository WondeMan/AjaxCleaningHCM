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
        public BankDto BankDto { get; set; }
    }
    public class BanksResponseDto : OperationStatusResponse
    {
        public List<BankDto> BankDtos { get; set; }
    }
    public class BankDto : OperationStatusResponse
    {
        public long Id { get; set; }
        [Required, Display(Name = "Bank Name")]
        public string BankName { get; set; }
        [Required, Display(Name = "Bank Code")]
        public string BankCode { get; set; }
        public MainCurrency MainCurrency { get; set; }
    }
}
