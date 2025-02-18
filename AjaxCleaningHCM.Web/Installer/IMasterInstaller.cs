using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AjaxCleaningHCM.Core.MasterData.Interface;
using AjaxCleaningHCM.Core.MasterData.Service;
using AjaxCleaningHCM.Domain.Models.MasterData;
using AjaxCleaningHCM.Core.Helper.Interface;
using AjaxCleaningHCM.Core.Helper.Service;

namespace AjaxCleaningHCM.Web.Installer
{
    public class IMasterInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IProduct,ProductService>();
            services.AddScoped<IMenuBuilder, MenuBuilderService>();

            services.AddScoped<IBank, BankService>();

            services.AddScoped<IEmployee, EmployeeService>();

            services.AddScoped<IBranch, BranchService>();
            services.AddScoped<IShift, ShiftService>();



        }
    }
}
