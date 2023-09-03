namespace Catfish.API.Repository.Services
{
    using Catfish.API.Repository.Interfaces;
    using CatfishExtensions.DTO;
    using CatfishExtensions.Interfaces;
    using System.Net.Mail;
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly string _fromEmail;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly bool _ssl;

        public EmailService(IConfiguration config)
        {
            _config = config;
            _fromEmail = _config.GetSection("EmailConfig:Sender").Value;
            _smtpServer = _config.GetSection("EmailConfig:Server").Value;
            _smtpPort = _config.GetValue<int>("EmailConfig:Port");
            _ssl = _config.GetValue<bool>("EmailConfig:SSL");
        }

        public EmailService(string fromEmail, string smtpServer, int smtpPort, bool ssl)
        {
            _fromEmail = fromEmail;
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _ssl = ssl;
        }

        public void SendEmail(Email email)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(_fromEmail);
                mailMessage.Subject = email.Subject;
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = "<p>" + email.Body + "</p>";
                foreach(string emailRecipient in email.ToRecipientEmail)
                    mailMessage.To.Add(emailRecipient);

                if (email.CcRecipientEmail != null)
                {
                    foreach (string emailRecipient in email.CcRecipientEmail)
                        mailMessage.CC.Add(emailRecipient);
                }

                if (email.BccRecipientEmail != null)
                {
                    foreach (string emailRecipient in email.BccRecipientEmail)
                        mailMessage.Bcc.Add(emailRecipient);
                }

                using (SmtpClient client = new SmtpClient(_smtpServer, _smtpPort))
                {
                    client.UseDefaultCredentials = true;
                    client.EnableSsl = _ssl;
                    client.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
               
            }
        }
    }
}
