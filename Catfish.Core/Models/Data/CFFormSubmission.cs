using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Core.Models.Data
{
    public class CFFormSubmission : CFDataObject
    {
        public static string TagName { get { return "form-submission"; } }
        public override string GetTagName() { return TagName; }

        public CFForm FormData
        {
            get
            {
                var xpath = "./data/" + CFForm.TagName;
                CFForm form = GetChildModels(xpath, Data).FirstOrDefault() as CFForm;
                return form;
            }
        }

        public void ReplaceFormData(XElement formData)
        {
            if (Data.Element("data") == null)
                Data.Add(new XElement("data"));
            else
            {
                var xpath = "./data/" + CFForm.TagName;
                XElement ele = GetChildElements(xpath, Data).FirstOrDefault();
                if (ele != null)
                    ele.Remove();
            }

            InsertChildElement("./data", formData);
        }

        public void UpdateFormData(CFForm src)
        {
            var xpath = "./data/" + CFForm.TagName;
            CFForm storedForm = GetChildModels(xpath, Data).FirstOrDefault() as CFForm;
            storedForm.UpdateValues(src);
        }
    }
}
