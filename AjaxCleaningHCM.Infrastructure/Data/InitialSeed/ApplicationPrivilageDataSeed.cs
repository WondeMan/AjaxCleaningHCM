
using AjaxCleaningHCM.Core.UserManagment.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace AjaxCleaningHCM.Infrastructure.Data.InitialSeed
{
    public class ApplicationPrivilageDataSeed
    {
        public List<Privilege> GetCommonPrivileges()
        {
            var privileges = new List<Privilege>()
            {
            //Account
            new Privilege() { Id = Guid.NewGuid().ToString(), Action = "Account-Index" },
            new Privilege() { Id = Guid.NewGuid().ToString(), Action = "Account-Register" },
            new Privilege() { Id = Guid.NewGuid().ToString(), Action = "Account-Lockout" },
            new Privilege() { Id = Guid.NewGuid().ToString(), Action = "Account-Details" },
            new Privilege() { Id = Guid.NewGuid().ToString(), Action = "Account-ForgotPassword" },
            new Privilege() { Id = Guid.NewGuid().ToString(), Action = "Account-ForgotPasswordConfirmation" },
            new Privilege() { Id = Guid.NewGuid().ToString(), Action = "Account-LogOff" },
            new Privilege() { Id = Guid.NewGuid().ToString(), Action = "Account-ResetPassword", Description = "Reset Password" },

            //Privileges
            new Privilege() { Id = Guid.NewGuid().ToString(), Action = "Privileges-Index" },
            new Privilege() { Id = Guid.NewGuid().ToString(), Action = "Privileges-Create" },
            new Privilege() { Id = Guid.NewGuid().ToString(), Action = "Privileges-Edit" },
            new Privilege() { Id = Guid.NewGuid().ToString(), Action = "Privileges-Delete" },
            new Privilege() { Id = Guid.NewGuid().ToString(), Action = "Privileges-DeleteConfirmed" },

            //Roles
            new Privilege() { Id = Guid.NewGuid().ToString(), Action = "Roles-Index" },
            new Privilege() { Id = Guid.NewGuid().ToString(), Action = "Roles-Create" },
            new Privilege() { Id = Guid.NewGuid().ToString(), Action = "Roles-Edit" },
            new Privilege() { Id = Guid.NewGuid().ToString(), Action = "Roles-Delete" },
            new Privilege() { Id = Guid.NewGuid().ToString(), Action = "Roles-DeleteConfirmed" },

            //AccessLog
            new Privilege() { Id = Guid.NewGuid().ToString(), Action = "AccessLog-Index" },
            
            //Shared
            new Privilege() { Id = Guid.NewGuid().ToString(), Action = "Shared-Index" },
            };

            return privileges;
        }

        public List<Privilege> GetAjaxCleaningHCMMemberPrivilages()
        {
            return null;
        }
        public List<Privilege> GetSeedData()
        {
            var privileges = new List<Privilege>();

            privileges.AddRange(GetCommonPrivileges());
            //privileges.AddRange(GetAjaxCleaningHCMMemberPrivilages());

            return privileges;
        }
    }

    public class ApplicationRoleDataSeed
    {
        public List<string> GetCommonRole()
        {
            var roles = new List<string>()
            {
                "Admin",
                "User"
            };
            return roles;
        }
    }
}
