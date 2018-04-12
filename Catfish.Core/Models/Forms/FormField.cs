using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Catfish.Core.Models.Attributes;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using Catfish.Core.Helpers;
using Catfish.Core.Validators;
using Catfish.Core.Models.Data;
using System.Web.Script.Serialization;

namespace Catfish.Core.Models.Forms
{
    [FormFieldRequired]
    public class FormField : XmlModel
    {
        public override string GetTagName() { return "field"; }
        public static char FileGuidSeparator = '|';

        [NotMapped]
        public List<FileDescription> Files
        {
            get
            {
                return GetChildModels("files/" + FileDescription.TagName).Select(c => c as FileDescription).ToList();
            }

            set
            {
                XElement filesElement = Data.Element("files");
                if (filesElement == null)
                {
                    filesElement = new XElement("files");
                    Data.Add(filesElement);
                }
                foreach (FileDescription fileDescription in value)
                {
                    filesElement.Add(fileDescription.Data);
                }

            }
        }


        //[Display(Name = "Name")]
        [ScriptIgnore]
        public IEnumerable<TextValue> MultilingualName
        {
            get
            {
                return GetNames(true);
            }

            set
            {
                SetName(value);
            }
        }

        [ScriptIgnore]
        [NotMapped]
        [Display(Name="Is Required")]
        public bool IsRequired
        {
            get
            {
                if (Data != null)
                {
                    var att = Data.Attribute("IsRequired");
                    return att != null ? att.Value == "true" : false;
                }
                return false;
            }

            set
            {
                Data.SetAttributeValue("IsRequired", value);
            }
        }

        [NotMapped]
        [ScriptIgnore]
        [Display(Name="Tooltip Help")]
        [DataType(DataType.MultilineText)]
        public string Help
        {
            get
            {
                return GetHelp();
            }

            set
            {
                SetHelp(value);
            }
        }

        [NotMapped]
        [ScriptIgnore]
        public IReadOnlyList<TextValue> Values
        {
            get
            {
                XElement valueWrapper = Data.Element("value");
                if (valueWrapper == null)
                    Data.Add(valueWrapper = new XElement("value"));

                return XmlHelper.GetTextValues(valueWrapper, true).ToList();
            }

            set
            {
                if (value != null)
                    SetTextValues(value);
            }
        }

        [DataType(DataType.MultilineText)]
        [NotMapped]
        [ScriptIgnore]
        public string Description
        {
            get
            {
                return GetDescription();
            }

            set
            {
                SetDescription(value);
            }
        }

        [ScriptIgnore]
        public IEnumerable<TextValue> MultilingualDescription
        {
            get
            {
                return GetDescription(true);
            }

            set
            {
                SetDescription(value);
            }
        }


        [NotMapped]
        [ScriptIgnore]
        public int Rank
        {
            get { return GetAttribute("rank", 0); }
            set { SetAttribute("rank", value); }
        }

        [NotMapped]
        [ScriptIgnore]
        public int Page
        {
            get { return GetAttribute("page", 0); }
            set { SetAttribute("page", value); }
        }

        public override void UpdateValues(XmlModel src)
        {
            XElement srcValueWrapper = src.Data.Element("value");
            if (srcValueWrapper == null)
                return;

            IEnumerable<XElement> srcText = srcValueWrapper.Elements("text");
            if (srcText.Count() == 0)
                return;

            XElement dstValeWrapper = Data.Element("value");
            if (dstValeWrapper == null)
                Data.Add(dstValeWrapper = new XElement("value"));
            else
            {
                //deleting all existing text elements from the destination
                foreach (var txt in dstValeWrapper.Elements("text").ToList())
                    txt.Remove();
            }

            //inserting clones of text elements in the src value wrapper
            foreach (var txt in srcValueWrapper.Elements("text"))
                dstValeWrapper.Add(new XElement(txt));
        }

        public bool IsPageBreak()
        {
            return typeof(PageBreak).IsAssignableFrom(GetType());
        }

    }
}
