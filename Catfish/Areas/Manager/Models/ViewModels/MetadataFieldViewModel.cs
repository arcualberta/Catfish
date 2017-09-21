using Catfish.Core.Models.Attributes;
using Catfish.Core.Models.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catfish.Areas.Manager.Models.ViewModels
{
    public class MetadataFieldViewModel : KoBaseViewModel
    {
        public string TypeLabel { get; set; }
        public string FieldType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsRequired { get; set; }
        public bool IsOptionField { get; set; }
        public string Options { get; set; }

        public MetadataFieldViewModel() { }

        public MetadataFieldViewModel(MetadataField src)
        {
            Name = src.Name;
            Description = src.Description;
            IsRequired = src.IsRequired;
            FieldType = src.GetType().AssemblyQualifiedName;
            IsOptionField = typeof(OptionsField).IsAssignableFrom(src.GetType());
            if (IsOptionField)
                Options = string.Join("\n", (src as OptionsField).Options.Select(op => op.Value));

            TypeLabelAttribute att = Attribute.GetCustomAttribute(src.GetType(), typeof(TypeLabelAttribute)) as TypeLabelAttribute;
            TypeLabel = att == null ? src.GetType().ToString() : att.Name;
        }

        public MetadataField InstantiateDataModel()
        {
            Type type = Type.GetType(FieldType, true);
            if (!typeof(MetadataField).IsAssignableFrom(type))
                throw new InvalidOperationException("Bad Type");

            MetadataField field = Activator.CreateInstance(type) as MetadataField;
            field.Name = Name;
            field.Description = Description;
            field.IsRequired = IsRequired;
            if (typeof(OptionsField).IsAssignableFrom(type))
            {
                //Creating option list separately and assigning it to the Options propery of the Options field
                //to make sure that the overridden setter method is invoked to save data in XML
                List<Option> optList = new List<Option>();
                (field as OptionsField).Options = CreateOptions(Options);
            }

            return field;
        }

        protected List<Option> CreateOptions(string newLineSeparatedPptions)
        {
            List<Option> optList = new List<Option>();
            if (!string.IsNullOrEmpty(newLineSeparatedPptions))
            {
                string[] options = newLineSeparatedPptions.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string opt in options)
                    optList.Add(new Option(opt, false));
            }
            return optList;
        }
    }
}