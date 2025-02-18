using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AjaxCleaningHCM.Infrastructure.Data
{
    public static class ApplicationDbContextExtensions
    {
        public static void ApplyCustomConfigurations(this ModelBuilder modelBuilder)
        {
            IdentityConfiguration.ConfigureIdentityTables(modelBuilder);
            IdentityConfiguration.ConfigureCustomIdentityTables(modelBuilder);
        }
    }

}
