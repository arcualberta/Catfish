using Catfish.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Core.Models.Forms
{
    public class SingleSelectOptionsField: OptionsField
    {
        [NotMapped]
        public string Value
        {
            get
            {
                XElement val = Data.Element("value");
                return val == null ? null : val.Value;
            }

            set
            {
                XElement val = Data.Element("value");
                if (val == null)
                    Data.Add(val = new XElement("value"));
                val.Value = value == null ? "" : value;
            }
        }
        public override void UpdateValues(XmlModel src)
        {
            //Unlike multi-select option fields such as check boxes, single select option fields such as radio buttons sends
            //the selected option as a value. Therefore, we need to find the corresponding value from the option set in the 
            //destination and set it's selected property to true (and deselect all other properties).

            var dstWrapper = Data.Element("options");
            if (dstWrapper == null)
                Data.Add(dstWrapper = new XElement("options"));

            var srcValueElement = src.Data.Element("value");
            string srcValue = srcValueElement == null ? null : srcValueElement.Value;
            string lang = Lang(null);
            //Iterating through all options in the destinatopn
            foreach (XElement opt in dstWrapper.Elements("option"))
            {
                bool selected = false;
                if(!string.IsNullOrEmpty(srcValue))
                {
                    IEnumerable<TextValue> dstValues = XmlHelper.GetTextValues(opt, false);
                    selected = dstValues.Where(d => d.LanguageCode == lang && d.Value == srcValue).Any();
                }
                opt.SetAttributeValue("selected", selected);
            }
        }

      
    }
}
