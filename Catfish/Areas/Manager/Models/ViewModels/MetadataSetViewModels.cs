using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Catfish.Core.Models.Metadata;
using Catfish.Core.Models.Attributes;

namespace Catfish.Areas.Manager.Models.ViewModels
{
    public class MetadataFieldType
    {
        public string FieldType { get; set; }
        public string Label { get; set; }
        public MetadataFieldType()
        {
            Label = "";
            FieldType = "";
        }
    }

    public class MetadataSetViewModel : KoBaseViewModel
    {
        public string TypeLabel { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<MetadataFieldViewModel> Fields { get; set; }

        public List<MetadataFieldType> FieldTypes { get { return GetFieldTypes(); } }
        public List<MetadataFieldType> SelectedFieldTypes { get; set; }

        public MetadataSetViewModel()
        {
        }

        public MetadataSetViewModel(MetadataSet src)
        {
            Id = src.Id;
            TypeLabelAttribute att = Attribute.GetCustomAttribute(src.GetType(), typeof(TypeLabelAttribute)) as TypeLabelAttribute;
            TypeLabel = att == null ? src.GetType().ToString() : att.Name;

            Name = src.Name;
            Description = src.Description;

            Fields = new List<MetadataFieldViewModel>();
            foreach (var field in src.Fields)
                Fields.Add(new MetadataFieldViewModel(field));
        }

        public void UpdateMetadataSet(MetadataSet dst)
        {
            dst.Name = Name;
            dst.Description = Description;

            //Updating fields. 
            //Note that it is necessary to create a new list of metadata fields
            //as follows and assign that list to the field list of dst. Simply emptying the field
            //list of dst and inserting fields into it would not work because such operation will
            //not utilize the overridden get and set methods.
            List<MetadataField> fields = new List<MetadataField>();
            foreach (MetadataFieldViewModel field in this.Fields)
                fields.Add(field.InstantiateDataModel());

            dst.Fields = fields;
        }

        private List<MetadataFieldType> mFieldTypes;
        public List<MetadataFieldType> GetFieldTypes()
        {
            if (mFieldTypes == null)
            {
                mFieldTypes = new List<MetadataFieldType>();
                mFieldTypes.Add(new MetadataFieldType());

                var fieldTypes = typeof(MetadataField).Assembly.GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(MetadataField))
                        && !t.CustomAttributes.Where(a => a.AttributeType.IsAssignableFrom(typeof(IgnoreAttribute))).Any())
                    .ToList();

                foreach (var t in fieldTypes)
                {
                    TypeLabelAttribute att = Attribute.GetCustomAttribute(t, typeof(TypeLabelAttribute)) as TypeLabelAttribute;

                    //We expect Metadata Fields that are usable by the interface
                    //to have a TypeLabel attribute to be defined (and labeled)
                    if (att != null)
                    {
                        mFieldTypes.Add(new MetadataFieldType()
                        {
                            FieldType = t.AssemblyQualifiedName,
                            Label = att.Name
                        });
                    }
                }
            }

            return mFieldTypes;
        }
    }
}