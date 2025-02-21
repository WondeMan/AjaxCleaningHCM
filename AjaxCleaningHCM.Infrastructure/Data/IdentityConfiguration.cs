using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AjaxCleaningHCM.Core.UserManagment.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace AjaxCleaningHCM.Infrastructure.Data
{
    // IdentityConfiguration.cs
    public static class IdentityConfiguration
    {
        public static void ConfigureIdentityTables(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole<string>>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        }

        public static void ConfigureCustomIdentityTables(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Role>().HasKey(r => r.Id);
            modelBuilder.Entity<User>().ToTable("Users").HasMany((User u) => u.UserRoles);
            modelBuilder.Entity<UserRole>();
            modelBuilder.Entity<Privilege>().ToTable("Privileges").HasKey(p => p.Id);
            modelBuilder.Entity<Role>().HasMany((Role r) => r.RolePrivileges);
            modelBuilder.Entity<RolePrivilege>().ToTable("RolePrivileges").HasKey(p => new { p.RoleId, p.PrivilegeId });
        }
    }

}
