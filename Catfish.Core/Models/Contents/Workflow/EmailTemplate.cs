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
        public readonly string SubjectField = "Subject";
        public readonly string BodyField = "Body";
        public readonly string RecipientsField = "Recipients";
        public EmailTemplate(XElement data) : base(data)
        {

        }

        public void Update(string emailSubject, string emailBody, string[] recipients, string contentLanguage = "en")
        {
            SetFieldValue<TextField>(SubjectField, DefaultLanguage, emailSubject, contentLanguage, true);
            SetFieldValue<TextArea>(BodyField, DefaultLanguage, emailBody, contentLanguage, true);
            SetFieldValue<TextField>(RecipientsField, DefaultLanguage, recipients, contentLanguage, true);
        }

        public void SetSubject(string val, string contentLanguage = "en") => SetFieldValue<TextField>(SubjectField, DefaultLanguage, val, contentLanguage);
        public string GetSubject(string contentLanguage = "en") => GetValue<TextField>(SubjectField, DefaultLanguage, contentLanguage);

        public void SetBody(string val, string contentLanguage = "en") => SetFieldValue<TextArea>(BodyField, DefaultLanguage, val, contentLanguage);
        public string GetBody(string contentLanguage = "en") => GetValue<TextArea>(BodyField, DefaultLanguage, contentLanguage);

        public void SetRecipients(string[] val) => SetFieldValue<TextField>(RecipientsField, "en", val, "en");
        public List<string> GetRecipients() => GetValues<TextField>(RecipientsField, DefaultLanguage, DefaultLanguage);

        public void UpdateRerefences(string key, string value)
        {
            var body = GetBody();
            SetBody(body.Replace(key, value));
        }
    }
}
