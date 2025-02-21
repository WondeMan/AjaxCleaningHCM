using Microsoft.Extensions.Configuration;
using System.IO;

namespace AjaxCleaningHCM.Infrastructure.AppConfig
{
    public class AppConfiguration
    {
        private readonly string connectionString = string.Empty;
        private readonly string emailSender = string.Empty;
        private readonly string smtpHost = string.Empty;
        private readonly string password = string.Empty;
        private readonly string ccEmails = string.Empty;

        public AppConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);

            var root = configurationBuilder.Build();

            connectionString = root.GetSection("ConnectionStrings").GetSection("AjaxCleaningHCMConnection").Value;
            emailSender = root.GetSection("EmailSettings").GetSection("EmailSender").Value;
            smtpHost = root.GetSection("EmailSettings").GetSection("SMTPHost").Value;
            password = root.GetSection("EmailSettings").GetSection("Password").Value;
            ccEmails = root.GetSection("EmailSettings").GetSection("CC").Value;
        }
        public string ConnectionString
        {
            get => connectionString;
        }

        public string EmailSender
        {
            get => emailSender;
        }

        public string SMTPHost
        {
            get => smtpHost;
        }

        public string Password
        {
            get => password;
        }

        public string CCEmails
        {
            get => ccEmails;
        }
    }
}
