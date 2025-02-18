using AjaxCleaningHCM.Core.UserManagment.Identity;
using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AjaxCleaningHCM.Core.Account.Interface
{
    public interface IRole
    {
        Task<List<Role>> GetAllAsync();
        Task<Role> GetByIdAsync(string id);
        Task<OperationStatusResponse> CreateAsync(Role request);
        Task<OperationStatusResponse> UpdateAsync(Role request);
        Task<OperationStatusResponse> DeleteAsync(string id);
        Task<List<IdentityUserRole<string>>> GetUserRoleByIdAsync(string id);
        Task<OperationStatusResponse> CreateRoleWithPrivilegeAsync(RoleViewModel model, string Privilege);
        Task<OperationStatusResponse> UpdateRoleWithPrivilegeAsync(RoleViewModel model, string oldRole, string privilege);
        Task<Role> GetByRoleNameAsync(string roleName);
    }
}
