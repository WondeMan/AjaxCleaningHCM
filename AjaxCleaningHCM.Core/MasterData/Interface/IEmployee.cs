using AjaxCleaningHCM.Core.Helper;
using AjaxCleaningHCM.Domain.DTO.MasterData.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace AjaxCleaningHCM.Core.MasterData.Interface
{
    public interface IEmployee : ICrud<EmployeeResponseDto, EmployeesResponseDto, EmployeeDto>
    {

    }
}
