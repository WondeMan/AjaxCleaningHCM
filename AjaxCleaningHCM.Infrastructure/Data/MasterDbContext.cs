using Microsoft.EntityFrameworkCore;
using AjaxCleaningHCM.Core.UserManagment.Identity;
using AjaxCleaningHCM.Domain.Models.MasterData;
using System;
using System.Collections.Generic;
using System.Text;
using AjaxCleaningHCM.Domain.Models.Helper;

namespace AjaxCleaningHCM.Infrastructure.Data
{
    public partial class ApplicationDbContext
    {
        public DbSet<Product> Products { get; set; }
        // Master Data
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Branch> Branchs { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<EmployeeHistory> EmployeeHistories { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<MenuBuilder> MenuBuilders { get; set; }
        public DbSet<EmployeeTermination> EmployeeTerminations { get; set; }


    }
}
