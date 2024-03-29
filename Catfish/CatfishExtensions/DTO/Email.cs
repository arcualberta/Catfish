﻿

namespace CatfishExtensions.DTO
{
    public class Email
    {
        public string FromEmail { get; set; }
        public string Body { get; set; }
        public string UserName { get; set; }
        public string Subject { get; set; }
        public List<string> ToRecipientEmail { get; set; }
        public List<string> CcRecipientEmail { get; set; }
        public List<string> BccRecipientEmail { get; set; }
    }
}
