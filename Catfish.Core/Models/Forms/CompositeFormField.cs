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

        public CompositeFormField()
        {
            Data.Add(new XElement("fields"));
            Data.Add(new XElement("header"));
            Data.Add(new XElement("footer"));
        }

        public override void UpdateValues(CFXmlModel src)
        {
            base.UpdateValues(src);

            CompositeFormField srcField = src as CompositeFormField;

            //Updating contents of all child fields
            foreach (FormField field in Fields)
            { // checkhere type of 
                var src_field = srcField.Fields.Where(x => x.Guid == field.Guid).FirstOrDefault();
                if (src_field != null)
                    field.UpdateValues(src_field);
            }

            //Updating contents of the header fields
            foreach (FormField field in Header)
            { // checkhere type of 
                var src_field = srcField.Header.Where(x => x.Guid == field.Guid).FirstOrDefault();
                if (src_field != null)
                    field.UpdateValues(src_field);
            }

            //Updating contents of the footer fields
            foreach (FormField field in Footer)
            { // checkhere type of 
                var src_field = srcField.Footer.Where(x => x.Guid == field.Guid).FirstOrDefault();
                if (src_field != null)
                    field.UpdateValues(src_field);
            }
        }

    }
}
