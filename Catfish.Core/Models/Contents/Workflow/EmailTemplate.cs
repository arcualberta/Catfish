using Catfish.Core.Models.Contents.Fields;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class EmailTemplate : FieldContainer
    {
        public readonly string DefaultLanguage = "en";
        public readonly string SubjectField = "en";
        public readonly string BodyField = "en";
        public readonly string RecipientsField = "en";
        public EmailTemplate(XElement data) : base(data)
        {

        }

        public void Update(string emailSubject, string emailBody, string[] recipients, string contentLanguage = "en")
        {
            SetFieldValue<TextField>(SubjectField, DefaultLanguage, emailSubject, contentLanguage, true);
            SetFieldValue<TextArea>(BodyField, DefaultLanguage, emailBody, contentLanguage, true);
            SetFieldValue<TextField>(RecipientsField, DefaultLanguage, recipients, contentLanguage, true);
        }

        public string GetSubject(string contentLanguage = "en") => GetValue<TextField>(SubjectField, DefaultLanguage, contentLanguage);
        public string GetBody(string contentLanguage = "en") => GetValue<TextArea>(BodyField, DefaultLanguage, contentLanguage);
        public List<string> GetRecipients() => GetValues<TextField>(RecipientsField, DefaultLanguage, DefaultLanguage);

    }
}
