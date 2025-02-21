using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AjaxCleaningHCM.Domain.Utils;
using AjaxCleaningHCM.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using AjaxCleaningHCM.Core.UserManagment.Identity;
using AjaxCleaningHCM.Infrastructure.Data.InitialSeed;
using static AjaxCleaningHCM.Domain.Enums.Common;

namespace AjaxCleaningHCM.Infrastructure.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly IServiceProvider serviceProvider;
        public IConfiguration Configuration { get; }

        public DbInitializer(IServiceProvider _serviceProvider, IConfiguration _configuration)
        {
            serviceProvider = _serviceProvider;
            Configuration = _configuration;
        }

        //Seed, Creat admin role and users, and assign privileges
        public async void Initialize()
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                //create database schema if none exists
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                var dbCreated = context.Database.EnsureCreated();

                if (!context.Users.Any())
                {

                    //Application privilages
                    var Privileges = new List<Privilege>();
                    foreach (var Privilege in new ApplicationPrivilageDataSeed().GetSeedData())
                    {
                        if (Privileges.Where(a => a.Action.Equals(Privilege.Action)).Count() == 0)
                            Privileges.Add(Privilege);
                    }
                    context.Privileges.AddRange(Privileges);
                    var rolesList = new List<Role>();
                    var _roleManager = serviceScope.ServiceProvider.GetService<RoleManager<Role>>();
                    foreach (var role in new ApplicationRoleDataSeed().GetCommonRole())
                    {
                        if (!await _roleManager.RoleExistsAsync(role.ToString()))
                        {
                            //Create Role
                            var result = await _roleManager.CreateAsync(new Role()
                            {
                                Name = role,
                                Description = role,
                                RecordStatus = RecordStatus.Active,
                                RegisteredBy = "System",
                                RegisteredDate = DateTime.Now,
                            });
                            if (!result.Succeeded)
                                return;
                        }
                    }
                    //var rolesList = new List<Role>();
                    ////If there is already an Administrator role, abort
                    //var _roleManager = serviceScope.ServiceProvider.GetService<RoleManager<Role>>();

                    //foreach (var role in Enum.GetValues(typeof(AppUserRole)))
                    //{
                    //    var description = role.ToString();
                    //    if (!await _roleManager.RoleExistsAsync(role.ToString()))
                    //    {
                    //        //Create Role
                    //        var result = await _roleManager.CreateAsync(new Role(role.ToString(), description, (AppUserRole)role));
                    //        rolesList.Add(new Role(role.ToString(), description, (AppUserRole)role));
                    //        if (!result.Succeeded)
                    //            return;
                    //    }
                    //    context.SaveChanges();
                    //}
                    //context.Roles.AddRange(rolesList);
                    //context.SaveChanges();
                    var roles = context.Roles.ToList();

                    //Admin Privileges
                    var rolePrivileges = new List<RolePrivilege>();
                    foreach (var ap in context.Privileges.ToList())
                    {
                        if (context.RolePrivileges.Count(r => r.PrivilegeId == ap.Id) == 0)
                        {
                            rolePrivileges.Add(
                                new RolePrivilege
                                {
                                    PrivilegeId = ap.Id,
                                    RoleId = roles.FirstOrDefault(r => r.Name == "Admin")?.Id
                                });
                        }
                    }
                    if (rolePrivileges.Count > 0)
                    {
                        context.AddRange(rolePrivileges);
                        context.SaveChanges();
                    }

                    var _userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();

                    //Create Admin USER/s
                    var usr = new User
                    {
                        UserName = Configuration.GetSection("UserSettings")["AdminUserName"],
                        Email = Configuration.GetSection("UserSettings")["AdminUserEmail"],
                        PhoneNumber = Configuration.GetSection("UserSettings")["AdminUserPhoneNumber"],
                        RegisteredDate = DateTime.Now,
                        RegisteredBy = "System",
                        RecordStatus = RecordStatus.Active,
                        FirstName = "Defualt",
                        LastName = "Defualt",
                        MiddleName = "Defualt",

                        //UserRole = role,
                        //EmployeeDetailsId = employee.Id,
                    };
                    var success = await _userManager.CreateAsync(usr, Configuration.GetSection("UserSettings")["DefaultPassword"]);
                    if (success.Succeeded)
                        await _userManager.AddToRoleAsync(await _userManager.FindByNameAsync(usr.UserName), "Admin");
                }
            }
        }
    }
}
