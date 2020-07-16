using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Services
{
    public class EmailService : IEmailService
    {




        //public interface IEmail
        //{
        //    void SendEmail(Email email);
        //}
        //public class Email
        //{
        //    public string FromEmail { get; set; }
        //    public string Body { get; set; }
        //    public string UserName { get; set; }
        //    public string Subject { get; set; }
        //    public string RecipientEmail { get; set; }
        //}
        //public class EmailService : IEmail
        //{
        //    private ICatfishAppConfiguration _config;
        //    static int portNumber = 587;
        //    static bool enableSSL = true;
        //    public EmailService(ICatfishAppConfiguration config)
        //    {
        //        _config = config;
        //    }
        //    public void SendEmail(Email email)
        //    {
        //        MailMessage mailMessage = new MailMessage();
        //        mailMessage.From = new MailAddress(email.FromEmail);
        //        mailMessage.To.Add(email.RecipientEmail);
        //        mailMessage.Subject = email.Subject;
        //        mailMessage.IsBodyHtml = true;
        //        mailMessage.Body = "<p>From: " + email.UserName + "</p><p>Email : " + email.FromEmail + "</p><p>Message: </p><p>" + email.Body + "</p>";

        //        using (SmtpClient client = new SmtpClient(_config.GetSmtpServer(), portNumber))
        //        {
        //            client.UseDefaultCredentials = true;
        //            client.EnableSsl = true;
        //            client.Send(mailMessage);
        //        }

        //    }
        //}
    }
}
