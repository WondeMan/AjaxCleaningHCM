using AjaxCleaningHCM.Core.UserManagment.Identity;
using AjaxCleaningHCM.Domain.Models.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AjaxCleaningHCM.Core.Account.Interface
{
    public interface IRolePrivilege
    {
        Task<List<RolePrivilege>> GetAllAsync();
        Task<RolePrivilege> GetByPrivilegeIdAsync(string id);
        Task<List<RolePrivilege>> GetByRoleIdAsync(string id);
        Task<OperationStatusResponse> CreateAsync(RolePrivilege request);
        Task<OperationStatusResponse> UpdateAsync(RolePrivilege request);
        Task<OperationStatusResponse> DeleteAsync(string id);
    }
}
