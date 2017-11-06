using Catfish.Core.Helpers;
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
        public override void UpdateValues(XmlModel src)
        {
            //Unlike multi-select option fields such as check boxes, single select option fields such as radio buttons sends
            //the selected option as a value. Therefore, we need to find the corresponding value from the option set in the 
            //destination and set it's selected property to true (and deselect all other properties).

            var dstWrapper = Data.Element("options");
            if (dstWrapper == null)
                Data.Add(dstWrapper = new XElement("options"));

            var srcValueElement = src.Data.Element("value");
            TextValue srcValue = XmlHelper.GetTextValues(srcValueElement, false).FirstOrDefault();

            //Iterating through all options in the destinatopn
            foreach (XElement opt in dstWrapper.Elements("option"))
            {
                bool selected = false;
                if(srcValue != null)
                {
                    IEnumerable<TextValue> dstValues = XmlHelper.GetTextValues(opt, false);
                    selected = dstValues.Where(d => d.LanguageCode == srcValue.LanguageCode && d.Value == srcValue.Value).Any();
                }
                opt.SetAttributeValue("selected", selected);
            }
        }

        ////////        public override void SetOptions(List<Option> options, string selection = null)
        ////////        {
        ////////            //makes sure the option list is built.
        ////////            base.SetOptions(options);

        ////////            if (string.IsNullOrEmpty(selection))
        ////////                return;
        /////////*
        ////////            List<XElement> optionList = GetOptionElements();
        ////////            string val_path = "./text[@xml:lang='" + Lang(lang) + "']";
        ////////            foreach (XElement opEle in optionList)
        ////////            {
        ////////                List<string> vals = GetChildElements(val_path, opEle).Select(txt => txt.Value).ToList();
        ////////                if (vals.Contains(selection))
        ////////                {
        ////////                    opEle.SetAttributeValue("selected", true);
        ////////                    break;
        ////////                }
        ////////            }
        ////////*/
        ////////        }
    }
}
