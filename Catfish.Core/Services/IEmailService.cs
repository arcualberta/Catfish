using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Core.Services
{
    public interface IEmailService
    {
        void SendEmail(Email email);
    }
    public class Email
    {
        public string FromEmail { get; set; }
        public string Body { get; set; }
        public string UserName { get; set; }
        public string Subject { get; set; }
        public string RecipientEmail { get; set; }
    }
}
