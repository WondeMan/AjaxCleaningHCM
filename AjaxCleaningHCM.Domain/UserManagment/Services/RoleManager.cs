using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using AjaxCleaningHCM.Domain.Identity;
using System;
using System.Threading.Tasks;
using AjaxCleaningHCM.Core.UserManagment.Identity;
using static AjaxCleaningHCM.Domain.Enums.Common;
using System.Data;

namespace AjaxCleaningHCM.Domain.UserManagment.Services
{
    public class RoleManager : IRoleManager
    {
        readonly IServiceProvider serviceProvider;
        RoleManager<Role> roleManager;

        public RoleManager(IServiceProvider _serviceProvider)
        {
            serviceProvider = _serviceProvider;

            var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            roleManager = serviceScope.ServiceProvider.GetService<RoleManager<Role>>();
        }

        public async Task<bool> CreateRole(string roleName, string description)
        {
            var result = await roleManager.CreateAsync(new Role()
            {
                Name = roleName,
                Description = description,
                RoleName = roleName,    
                RecordStatus = RecordStatus.Active,
                //RegisteredBy = "System",
                RegisteredDate = DateTime.Now,
            });

            return result.Succeeded;
        }

        public async Task<bool> RoleExists(string roleName)
        {
            return await roleManager.RoleExistsAsync(roleName);
        }
    }
}
