using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Microsoft.Extensions.Hosting;

namespace AjaxCleaningHCM.Core.Utils
{
    public class Keys
    {
        private IConfiguration Configuration { get; }
        private readonly IHostEnvironment env;

        LogWriter logWriter { get; set; }
        string path { get; set; }

        public Keys(IConfiguration _configuration, IHostEnvironment env)
        {
            this.env = env;
            Configuration = _configuration;
            logWriter = new LogWriter();
            path = Path.Combine(GetPathLevel1(), GetPathLevel2());
        }

        public string GetGeneratedExcelFullPath()
        {
            return GetLocalPath(Path.Combine(path, GetGeneratedExcelPath()));
        }

        public string GetLocalPath(string localPath)
        {
            if (!Directory.Exists(localPath))
                Directory.CreateDirectory(localPath);

            return localPath;
        }

        public string GetPathLevel1()
        {
            return Configuration.GetSection("AjaxCleaningHCMUploadPathSettings")["PathLevel1"];
        }
        public string GetPathLevel2()
        {
            return Configuration.GetSection("AjaxCleaningHCMUploadPathSettings")["PathLevel2"];
        }

        public string GetGeneratedExcelPath()
        {
            return Configuration.GetSection("AjaxCleaningHCMUploadPathSettings")["GeneratedExcelPath"];
        }
    }
}
