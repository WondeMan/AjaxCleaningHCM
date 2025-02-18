using AjaxCleaningHCM.Core.Helper.Interface;
using AjaxCleaningHCM.Core.UserManagment.Identity;
using AjaxCleaningHCM.Domain.Helpers;
using AjaxCleaningHCM.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using static AjaxCleaningHCM.Domain.Enums.Common;

namespace AjaxCleaningHCM.Core.Helper.Service
{
    public class RegisterPreviliegeService : IRegisterPreviliege
    {
        private readonly ApplicationDbContext _context;
        public RegisterPreviliegeService(ApplicationDbContext context)
        {
            _context = context;
        }
        public void RegisterPrivileges(Assembly assembly)
        {
            var privileges = _context.Privileges.ToList();
            var controllerWithActions = new List<ControllerWithAction>();
            List<Privilege> applicationPrivileges = new List<Privilege>();
            var controllerTypes = assembly.GetTypes().Where(type => typeof(ControllerBase).IsAssignableFrom(type) && !type.IsAbstract);
            foreach (var controllerType in controllerTypes)
            {
                var controllerWithAction = new ControllerWithAction
                {
                    ControllerType = controllerType,
                    ControllerName = controllerType.Name.Replace("Controller", string.Empty),
                    Actions = new List<string>()
                };
                var methods = controllerType.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public)
                                          .Where(method => !method.IsSpecialName);
                foreach (var method in methods)
                {
                    controllerWithAction.Actions.Add(method.Name);
                }
                controllerWithActions.Add(controllerWithAction);
            }
            foreach (var controllerType in controllerWithActions)
            {
                foreach (var action in controllerType.Actions)
                {
                    string claim = controllerType.ControllerName + "-" + action;
                    var privilege = privileges.Where(c => c.Action.ToLower() == claim.ToLower()).FirstOrDefault();
                    if (privilege == null)
                    {
                        if (applicationPrivileges.Select(c => c.Action).Contains(claim))
                            continue;
                        applicationPrivileges.Add(new Privilege
                        {
                            Id = Guid.NewGuid().ToString(),
                            Action = claim,
                            Description = action,
                            RecordStatus = RecordStatus.Active,
                            RegisteredDate = DateTime.Now,
                            TimeZoneInfo = "E. Africa Standard Time"
                        });
                    }
                }
            }
            if (applicationPrivileges.Count > 0)
            {
                _context.AddRange(applicationPrivileges);
                _context.SaveChanges();
            }
        }
    }
}
