using AjaxCleaningHCM.Core.UserManagment.Identity;
using AjaxCleaningHCM.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Net;

namespace AjaxCleaningHCM.Core.Utils
{
    public class AccessLog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        [Display(Name = "User Name ")]
        public string UserName { get; set; }

        [Display(Name = "Full Name ")]
        public string FullName { get; set; }

        [Display(Name = "Action Date ")]
        public DateTime ActionDate { get; set; }

        [Display(Name = "Statement Taken ")]
        public string StatementTaken { get; set; }

        [Display(Name = "Action")]
        public string ActionDid { get; set; }

        [Display(Name = "Client Computer ")]
        public string FromWhichComputer { get; set; }

        [Display(Name = "User Group ")]
        public string UserGroup { get; set; }//its just like ROLE

        [Display(Name = "Functionality Type ")]
        public string FunctionalityType { get; set; }

        [NotMapped]
        private ApplicationDbContext context;
        public AccessLog(ApplicationDbContext _applicationDbContext)
        {
            context = _applicationDbContext;
        }
        public AccessLog()
        { }

        public bool Save(string actionDid, string userName, string statmentTaken, string hostAddress, string functionalityType)
        {

            try
            {
                User user = context.Users.FirstOrDefault(con => con.UserName == userName);

                var userRole = context.UserRoles.Where(ur => ur.UserId == user.Id).FirstOrDefault();
                string role = "";

                if (userRole != null)
                {
                    var roles = context.Roles.Where(r => r.Id == userRole.RoleId).FirstOrDefault();
                    if (roles != null)
                        role = roles.Name;
                }

                //context.AccessLogs.Add(new AccessLog
                //{
                //    ActionDate = DateTime.Now,
                //    ActionDid = actionDid,
                //    FromWhichComputer = Utility.HostName(hostAddress),
                //    //FullName = user.EmployeeDetails.FullName,
                //    FunctionalityType = functionalityType,
                //    StatementTaken = statmentTaken,
                //    UserGroup = role,
                //    UserName = user.UserName
                //});

                context.SaveChanges();
                Utility.Logger(statmentTaken);
                return true;
            }
            catch (Exception e)
            {
                Utility.Logger("Error happened with error message " + e.Message + " when executing statment " + statmentTaken);
                return false;
            }
        }
    }

    public enum FunctionalityType
    {
        Account = 1,
        Role,
        Privillage
    }

    public static class GlobalVariables
    {
        public static string logPath = @"C:\EthiopianWebApplications\AppTemplate\";
        static GlobalVariables()
        {
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }
        }
    }


    public static class Utility
    {
        public static bool Logger(string message)
        {
            try
            {
                StreamWriter writer = new StreamWriter(GlobalVariables.logPath + "BSCTemplateLog " + DateTime.Now.ToString("yy-MM-dd tt") + ".txt", true);
                writer.WriteLine(message);
                writer.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string HostName(string userHostAddress)
        {
            try
            {
                string hostName = Dns.GetHostEntry(userHostAddress).HostName;
                return hostName;
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
