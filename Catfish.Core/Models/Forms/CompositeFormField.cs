using Catfish.Core.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Core.Models.Forms
{
    [CFTypeLabel("Composite form field")]
    public class CompositeFormField : FormField
    {
        public enum eStepState
        {
            None = 0,
            [Display(Name = "Step Through")]
            StepThrough,
            Accumulative
        }

        [NotMapped]
        public bool Shuffle
        {
            get
            {
                return GetAttribute("shuffle", false);
            }

            set
            {
                SetAttribute("shuffle", value);
            }
        }

        [NotMapped]
        public eStepState StepState
        {
            get
            {
                return (eStepState)GetAttribute("stepThroughChildren", 0);
            }

            set
            {
                SetAttribute("stepThroughChildren", (int)value);
            }
        }

        [NotMapped]
        [IgnoreDataMember]
        public IReadOnlyList<FormField> Header
        {
            get
            {
                return GetChildModels("header/field", Data).Select(c => c as FormField).ToList();
            }

            set
            {
                //Removing all children inside the metadata set element
                RemoveAllElements("header/field", Data);

                foreach (FormField ms in value)
                    InsertChildElement("./header", ms.Data);
            }
        }

        [NotMapped]
        [IgnoreDataMember]
        public IReadOnlyList<FormField> Footer
        {
            get
            {
                return GetChildModels("footer/field", Data).Select(c => c as FormField).ToList();
            }

            set
            {
                //Removing all children inside the metadata set element
                RemoveAllElements("footer/field", Data);

                foreach (FormField ms in value)
                    InsertChildElement("./footer", ms.Data);
            }
        }

        ////[NotMapped]
        ////public string Header
        ////{
        ////    get
        ////    {
        ////        return GetChildText("header", Data, null);
        ////    }

        ////    set
        ////    {
        ////        SetChildText("header", value, Data, null);
        ////    }
        ////}

        ////[NotMapped]
        ////public string Footer
        ////{
        ////    get
        ////    {
        ////        return GetChildText("footer", Data, null);
        ////    }

        ////    set
        ////    {
        ////        SetChildText("footer", value, Data, null);
        ////    }
        ////}

        public CompositeFormField()
        {
            Data.Add(new XElement("fields"));
            Data.Add(new XElement("header"));
            Data.Add(new XElement("footer"));
        }

        [NotMapped]
        [IgnoreDataMember]
        public IReadOnlyList<FormField> Fields
        {
            get
            {
                return GetChildModels("fields/field", Data).Select(c => c as FormField).ToList();
            }

            set
            {
                //Removing all children inside the metadata set element
                RemoveAllElements("fields/field", Data);

                foreach (FormField ms in value)
                    InsertChildElement("./fields", ms.Data);
            }
        }
    }
}
