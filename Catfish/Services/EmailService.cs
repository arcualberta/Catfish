using Catfish.Core.Services;
using Catfish.Helper;
using ElmahCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Catfish.Services
{
    public class EmailService : IEmailService
    {


        private ICatfishAppConfiguration _config;
        static int portNumber = 587;
        static bool enableSSL = true;
        private readonly ErrorLog _errorLog;
        public EmailService(ICatfishAppConfiguration config, ErrorLog errorLog)
        {
            _config = config;
            _errorLog = errorLog;
        }
        /// <summary>
        /// Sending email from contact form Block
        /// </summary>
        /// <param name="email"></param>
        public void SendEmail(Email email)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(_config.GetSmtpEmail()); 
                mailMessage.To.Add(email.RecipientEmail);
                mailMessage.Subject = email.Subject;
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = "<p>" + email.Body + "</p>";

                using (SmtpClient client = new SmtpClient(_config.GetSmtpServer(), portNumber))
                {
                    client.UseDefaultCredentials = true;
                    client.EnableSsl = true;
                    client.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
            }
        }

    }
}
