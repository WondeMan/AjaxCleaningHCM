using AjaxCleaningHCM.Core.Account.Interface;
using AjaxCleaningHCM.Core.Account.Service;
using AjaxCleaningHCM.Core.Helper.Interface;
using AjaxCleaningHCM.Core.Helper.Service;
using AjaxCleaningHCM.Core.MasterData.Interface;
using AjaxCleaningHCM.Core.MasterData.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AjaxCleaningHCM.Web.Installer
{
    public class IAccountInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPrivilege, PrivilegeService>();
            services.AddScoped<IRole, RoleService>();
            services.AddScoped<IRolePrivilege, RolePrivilegeService>();
            services.AddScoped<IUserAccount, UserAccountService>();
            services.AddScoped<IRegisterPreviliege, RegisterPreviliegeService>();

        }
    }
}
