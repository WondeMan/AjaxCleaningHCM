using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AjaxCleaningHCM.Domain.Utils
{
    public interface IEmailSender
    {
        object SendEmail(string message, List<string> reciever, List<string> emailsInCopy, string subject, string notificationType);
        //Task<object> SendEmailWithAttachmentAsync(string message, List<string> recievers, string subject, MemoryStream receiptStream);
    }
}
