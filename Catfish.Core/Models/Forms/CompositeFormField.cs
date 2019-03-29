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
           // SelectedFormFieldHeader = new List<FormFieldType>();
           // SelectedFormFieldTypeList = new List<string>();
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

        [NotMapped]
        [IgnoreDataMember]
        public List<FormFieldType> FormFields
        {
            get
            {
                //List<FormField> formFieldList = new List<FormField>();
                List<FormFieldType> mFieldTypes = new List<FormFieldType>();
                var fieldTypes = typeof(FormField).Assembly.GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(FormField))
                        && !t.CustomAttributes.Where(a => a.AttributeType.IsAssignableFrom(typeof(CFIgnoreAttribute))).Any())
                    .ToList();

                foreach (var t in fieldTypes)
                {
                    CFTypeLabelAttribute att = Attribute.GetCustomAttribute(t, typeof(CFTypeLabelAttribute)) as CFTypeLabelAttribute;

                    //We expect Metadata Fields that are usable by the interface
                    //to have a TypeLabel attribute to be defined (and labeled)
                    if (att != null)
                    {
                        mFieldTypes.Add(new FormFieldType()
                        {
                            FieldType = t.AssemblyQualifiedName,
                            Label = att.Name
                        });
                    }
                }

                return mFieldTypes;
            }

        }


        //[NotMapped]
        //public List<FormFieldType> SelectedFormFieldHeader { get; set; }
        //[NotMapped]
        //public List<string> SelectedFormFieldTypeList { get; set; }
        [NotMapped]
        public string SelectedFormFieldType { get; set; }

        public class FormFieldType
        {
            public string FieldType { get; set; }
            public string Label { get; set; }
            public FormFieldType()
            {
                Label = "";
                FieldType = "";
            }
        }
        //[NotMapped]
        //public FieldTypeViewModel fieldTypeViewModel { get; set; }


    }
}
