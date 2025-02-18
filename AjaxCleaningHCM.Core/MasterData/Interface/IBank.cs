using AjaxCleaningHCM.Core.Helper;
using AjaxCleaningHCM.Domain.DTO.MasterData.Request;
using AjaxCleaningHCM.Domain.DTO.MasterData.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace AjaxCleaningHCM.Core.MasterData.Interface
{
    public interface IBank : ICrud<BankResponseDto, BanksResponseDto, BankDto>
    {

    }
}
