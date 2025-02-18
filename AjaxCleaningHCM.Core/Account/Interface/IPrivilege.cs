using AjaxCleaningHCM.Core.UserManagment.Identity;
using AjaxCleaningHCM.Domain.Models.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AjaxCleaningHCM.Core.Account.Interface
{
    public interface IPrivilege
    {
        Task<List<Privilege>> GetAllAsync();
        Task<Privilege> GetByIdAsync(string id);
        Task<OperationStatusResponse> CreateAsync(Privilege request);
        Task<OperationStatusResponse> UpdateAsync(Privilege request);
        Task<OperationStatusResponse> DeleteAsync(string id);
        UserRolePrivilege GetUserRolesPrivileges();

    }
}
