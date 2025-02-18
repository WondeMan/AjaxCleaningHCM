using AjaxCleaningHCM.Domain.Utils;
using AjaxCleaningHCM.Domain.Models;
using AjaxCleaningHCM.Infrastructure.AppConfig;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using static AjaxCleaningHCM.Domain.Enums.Common;

namespace AjaxCleaningHCM.Core.Utils
{
    public class EmailConfiguration
    {
        public string FromEmail { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
    }

    public class EmailSender : IEmailSender
    {
        public bool IsEmailValid(string email)
        {
            string pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
            return !string.IsNullOrEmpty(email) && Regex.IsMatch(email, pattern);
        }

        public object SendEmail(string message, List<string> receivers, List<string> emailsInCopy, string subject, string notificationType)
        {
            //receivers = new List<string> { "gebreegziabherg@ethiopianairlines.com", "LubabaA@ethiopianairlines.com", "HENOKSH@ethiopianairlines.com" };
            try
            {
                if (receivers?.Count > 0)
                {
                    AppConfiguration appConfiguration = new AppConfiguration();

                    MailMessage emailMessage = new MailMessage();
                    emailMessage.IsBodyHtml = true;
                    emailMessage.From = new MailAddress(appConfiguration.EmailSender);

                    foreach (var reciever in receivers)
                        emailMessage.To.Add(new MailAddress(reciever.Trim()));

                    if (emailsInCopy != null && emailsInCopy.Count == 0)
                    {
                        foreach (var copy in emailsInCopy)
                            emailMessage.CC.Add(new MailAddress(copy.Trim()));
                    }

                    emailMessage.Body = message;
                    emailMessage.Subject = subject;

                    SmtpClient smtpClient = new SmtpClient
                    {
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(appConfiguration.EmailSender, appConfiguration.Password),
                        Port = 587,
                        Host = appConfiguration.SMTPHost,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        //EnableSsl = true
                    };

                     smtpClient.Send(emailMessage);

                    return new OperationResult
                    {
                        Status = OperationStatus.SUCCESS,
                        Message = Enum.GetName(typeof(OperationStatus), OperationStatus.SUCCESS)
                    };
                }

                return new OperationResult
                {
                    Status = OperationStatus.ERROR,
                    Message = "Reciever email address is not given."
                };
            }
            catch (Exception ex)
            {
                return new OperationResult
                {
                    ex = ex,
                    Message = "Exception Occured on SendEmail",
                    Status = OperationStatus.ERROR
                };
            }
        }

        //public async Task<object> SendEmailWithAttachmentAsync(string message, List<string> recievers, string subject, MemoryStream receiptStream)
        //{
        //    try
        //    {
        //        if (recievers?.Count > 0)
        //        {
        //            MailMessage emailMessage = new MailMessage();
        //            EmailConfiguration emailConfiguration = new EmailConfiguration
        //            {
        //                FromEmail = "ethiopianairlinesservices@ethiopianairlines.com",
        //                Host = "outlook.office365.com",
        //                Password = "EtOffice365"
        //            };

        //            MailAddress from = new MailAddress(emailConfiguration.FromEmail, "SkyLight Reservation Notification");
        //            emailMessage.From = from;

        //            foreach (var reciever in recievers)
        //                emailMessage.To.Add(reciever);

        //            emailMessage.Subject = subject;
        //            emailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.Never;
        //            emailMessage.Body = message;

        //            //emailMessage.CC.Add("reservation@ethiopianskylighthotel.com");
        //            //emailMessage.Bcc.Add("mulugetan@ethiopianairlines.com");
        //            //emailMessage.Bcc.Add("fishat@ethiopianairlines.com");

        //            //Attachment file
        //            if (receiptStream != null && receiptStream.Length > 0)
        //            {
        //                receiptStream.Position = 0;
        //                Attachment receiptAttachment = new Attachment(receiptStream, new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Application.Pdf));
        //                receiptAttachment.ContentDisposition.FileName = "SDMIS Confirmation.pdf";
        //                emailMessage.Attachments.Add(receiptAttachment);
        //            }

        //            emailMessage.IsBodyHtml = true;
        //            SmtpClient smtpClient = new SmtpClient
        //            {
        //                UseDefaultCredentials = false,
        //                Credentials = new NetworkCredential("ethiopianairlinesservices@ethiopianairlines.com", emailConfiguration.Password),
        //                Port = 25,
        //                Host = emailConfiguration.Host,
        //                EnableSsl = true,
        //                DeliveryMethod = SmtpDeliveryMethod.Network
        //            };

        //            try
        //            {
        //                smtpClient.SendAsync(emailMessage, null);

        //                return new DataObjects.Models.Others.OperationResult
        //                {
        //                    Status = OperationStatus.SUCCESS,
        //                    Message = Enum.GetName(typeof(OperationStatus), OperationStatus.Ok)
        //                };
        //            }
        //            catch (Exception ex)
        //            {
        //                return new DataObjects.Models.Others.OperationResult
        //                {
        //                    ex = ex,
        //                    Status = OperationStatus.ERROR,
        //                    Message = Enum.GetName(typeof(OperationStatus), OperationStatus.ERROR)
        //                };
        //            }
        //        }

        //        return new DataObjects.Models.Others.OperationResult
        //        {
        //            Status = OperationStatus.NOT_OK,
        //            Message = "Reciever email address is not given."
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new DataObjects.Models.Others.OperationResult
        //        {
        //            ex = ex,
        //            Message = "Exception Occured on SendEmail",
        //            Status = OperationStatus.ERROR
        //        };
        //    }
        //}
    }
}
