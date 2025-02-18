namespace AjaxCleaningHCM.Domain.Utils
{
    public interface ILogWriter
    {
        void CreateLog(string logText, string moduleName, string logName);
    }
}
