using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Core.Models.Data
{
    public class FormSubmission : DataObject
    {
        public static string TagName { get { return "form-submission"; } }
        public override string GetTagName() { return TagName; }

        public Form FormData
        {
            get
            {
                var xpath = "./data/" + Form.TagName;
                Form form = GetChildModels(xpath, Data).FirstOrDefault() as Form;
                return form;
            }
        }

        public void ReplaceFormData(XElement formData)
        {
            if (Data.Element("data") == null)
                Data.Add(new XElement("data"));
            else
            {
                var xpath = "./data/" + Form.TagName;
                XElement ele = GetChildElements(xpath, Data).FirstOrDefault();
                if (ele != null)
                    ele.Remove();
            }

            InsertChildElement("./data", formData);
        }

        public void UpdateFormData(Form src)
        {
            var xpath = "./data/" + Form.TagName;
            Form storedForm = GetChildModels(xpath, Data).FirstOrDefault() as Form;
            storedForm.UpdateValues(src);
            if (!string.IsNullOrEmpty(src.ReferenceCode))
                storedForm.ReferenceCode = src.ReferenceCode;
        }
    }
}
