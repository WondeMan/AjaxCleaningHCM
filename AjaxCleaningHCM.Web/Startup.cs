using FluentValidation.AspNetCore;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AjaxCleaningHCM.Domain.Identity;
using AjaxCleaningHCM.Domain.Utils;
using AjaxCleaningHCM.Core.UserManagment.Identity;
using AjaxCleaningHCM.Domain.UserManagment.Services;
using System;
using AjaxCleaningHCM.Infrastructure.Data;
using AjaxCleaningHCM.Core.Utils;
using AjaxCleaningHCM.Core.UserManagment.Services;
using AjaxCleaningHCM.Web.Installer;
using AjaxCleaningHCM.Core.Helper.Utils;
using AjaxCleaningHCM.Core.Helper.Interface;
using System.Reflection;
using AjaxCleaningHCM.Infrastructure.Data.InitialSeed;

namespace AjaxCleaningHCM
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            services.InstallServicesInAssembly(Configuration);
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("AjaxCleaningHCMConnection"),
                b => b.CommandTimeout(900).EnableRetryOnFailure().MigrationsAssembly("AjaxCleaningHCM.Web")).EnableSensitiveDataLogging()
                );

            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("AjaxCleaningHCMConnection")));
            services.AddHangfireServer();

            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddRazorPages();

            services.AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;

                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(20);
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IUserManager, UserManager>();
            services.AddTransient<IRoleManager, RoleManager>();

            //services.AddScoped<IBIDataSync, BIDataSync>();

            //Seed a defaut Account (User, Role and previlaege) 
            services.AddScoped<IDbInitializer, DbInitializer>();

            services.AddMvc().AddFluentValidation(fv =>
            {
                fv.ImplicitlyValidateChildProperties = true;
                fv.RegisterValidatorsFromAssemblyContaining<Startup>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddMemoryCache();
            services.AddAuthentication().AddCookie(o =>
            {
                o.ExpireTimeSpan = TimeSpan.FromMinutes(20);

            });
            services.AddSession(options =>
            {
                //Session time - 5 hours
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
            });
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env,
            IDbInitializer dbInitializer,
            IRecurringJobManager irecurringJobManager,
            IServiceProvider serviceProvider)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                var controllerCollector = services.GetRequiredService<IRegisterPreviliege>();
                var assembly = Assembly.GetExecutingAssembly();
                controllerCollector.RegisterPrivileges(assembly);
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                MasterDataSeeder.Seed(context);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors("AllowAll");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            dbInitializer.Initialize();

            app.UseRouting();
            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{area=AccountManagement}/{controller=Account}/{action=Login}");

                endpoints.MapRazorPages();
            });
        }
    }
}
