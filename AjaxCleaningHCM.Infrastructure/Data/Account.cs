using Microsoft.EntityFrameworkCore;
using AjaxCleaningHCM.Core.UserManagment.Identity;
using AjaxCleaningHCM.Domain.Models.MasterData;
using System;
using System.Collections.Generic;
using System.Text;

namespace AjaxCleaningHCM.Infrastructure.Data
{
    public partial class ApplicationDbContext
    {
        public DbSet<Privilege> Privileges { get; set; }
        public DbSet<RolePrivilege> RolePrivileges { get; set; }
    }
}
