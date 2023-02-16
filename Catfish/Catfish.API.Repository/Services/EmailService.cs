namespace Catfish.API.Repository.Services
{
    using Catfish.API.Repository.Interfaces;
    using System.Net.Mail;
    public class EmailService
    {
        private ICatfishAppConfiguration _config;
        static int portNumber = 587;
        static bool enableSSL = true;

        public EmailService(ICatfishAppConfiguration config)
        {
            _config = config;
        }
        public void SendEmail(Email email)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(_config.GetSmtpEmail());
                mailMessage.To.Add(email.ToRecipientEmail);
                mailMessage.CC.Add(email.CcRecipientEmail);
                mailMessage.Bcc.Add(email.BccRecipientEmail);
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
               
            }
        }
    }
}
