using AjaxCleaningHCM.Core.Helper.Service.Repository;
using AjaxCleaningHCM.Core.Helper;
using AjaxCleaningHCM.Domain.Models.MasterData;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using AjaxCleaningHCM.Domain.DTO.MasterData;
using AjaxCleaningHCM.Core.MasterData.Service;

namespace AjaxCleaningHCM.Web.Installer
{
    public class CrudInstaller: IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {

        }
    }
}
