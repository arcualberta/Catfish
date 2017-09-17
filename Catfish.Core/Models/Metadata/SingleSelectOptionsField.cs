using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Core.Models.Metadata
{
    public class SingleSelectOptionsField: OptionsField
    {
        public override void SetOptions(List<Option> options, string lang, string selection = null)
        {
            //makes sure the option list is built.
            base.SetOptions(options, lang);

            if (string.IsNullOrEmpty(selection))
                return;

            List<XElement> optionList = GetOptionElements();
            string val_path = "./text[@xml:lang='" + Lang(lang) + "']";
            foreach (XElement opEle in optionList)
            {
                List<string> vals = GetChildElements(val_path, opEle).Select(txt => txt.Value).ToList();
                if (vals.Contains(selection))
                {
                    opEle.SetAttributeValue("selected", true);
                    break;
                }
            }
        }
    }
}
