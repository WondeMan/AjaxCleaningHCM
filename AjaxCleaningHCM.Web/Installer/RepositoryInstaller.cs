using AjaxCleaningHCM.Core.Helper.Service.Repository;
using AjaxCleaningHCM.Core.Helper;
using AjaxCleaningHCM.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Diagnostics;
using System;
using AjaxCleaningHCM.Domain.Models.MasterData;
using Microsoft.Extensions.Configuration;
using AjaxCleaningHCM.Core.MasterData.Interface;
using AjaxCleaningHCM.Core.MasterData.Service;
using AjaxCleaningHCM.Domain.Models.Helper;

namespace AjaxCleaningHCM.Web.Installer
{
    public class RepositoryInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IRepositoryBase<MenuBuilder>, RepositoryBase<MenuBuilder>>();
            services.AddScoped<IRepositoryBase<Employee>, RepositoryBase<Employee>>();

            services.AddScoped<IRepositoryBase<Bank>, RepositoryBase<Bank>>();
            services.AddScoped<IRepositoryBase<Shift>, RepositoryBase<Shift>>();
            services.AddScoped<IRepositoryBase<Branch>, RepositoryBase<Branch>>();
            services.AddScoped<IRepositoryBase<Department>, RepositoryBase<Department>>();
            services.AddScoped<IRepositoryBase<EmployeeHistory>, RepositoryBase<EmployeeHistory>>();
            services.AddScoped<IRepositoryBase<Product>, RepositoryBase<Product>>();
            services.AddScoped<IRepositoryBase<EmployeeTermination>, RepositoryBase<EmployeeTermination>>();
            services.AddScoped<IRepositoryBase<Payroll>, RepositoryBase<Payroll>>();
            services.AddScoped<IRepositoryBase<DisciplineCategory>, RepositoryBase<DisciplineCategory>>();
            services.AddScoped<IRepositoryBase<EmployeeDiscipline>, RepositoryBase<EmployeeDiscipline>>();
            services.AddScoped<IRepositoryBase<LeaveType>, RepositoryBase<LeaveType>>();
            services.AddScoped<IRepositoryBase<LeaveRequest>, RepositoryBase<LeaveRequest>>();
            services.AddScoped<IRepositoryBase<IssueRegistration>, RepositoryBase<IssueRegistration>>(); 




        }
    }
}
