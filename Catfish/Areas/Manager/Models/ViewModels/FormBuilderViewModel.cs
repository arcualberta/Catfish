using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Catfish.Core.Models.Forms;
using Catfish.Core.Models.Attributes;
using Catfish.Core.Models;

namespace Catfish.Areas.Manager.Models.ViewModels
{
    public class FormBuilderViewModel : KoBaseViewModel
    {
        public string TypeLabel { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Guid { get; set; }
        public List<FormFieldViewModel> Fields { get; set; }

        public List<FormFieldType> FieldTypes { get { return GetFieldTypes(); } }
        public List<FormFieldType> SelectedFieldTypes { get; set; }

        public bool ShowFieldDescriptions { get; set; }

        public FormBuilderViewModel()
        {
            Fields = new List<FormFieldViewModel>();
            SelectedFieldTypes = new List<FormFieldType>();
        }

        public FormBuilderViewModel(AbstractForm src)
        {
            Id = src.Id;
            TypeLabelAttribute att = Attribute.GetCustomAttribute(src.GetType(), typeof(TypeLabelAttribute)) as TypeLabelAttribute;
            TypeLabel = att == null ? src.GetType().ToString() : att.Name;

            Name = src.Name;
            Description = src.Description;
            Guid = src.Guid;

            Fields = new List<FormFieldViewModel>();
            foreach (var field in src.Fields)
                Fields.Add(new FormFieldViewModel(field));

            Fields = Fields.OrderBy(f => f.Rank).ToList();
        }

        public override void UpdateDataModel(object dataModel, CatfishDbContext db)
        {
            AbstractForm dst = dataModel as AbstractForm;
            dst.Name = Name;
            dst.Description = Description;
            dst.Guid = Guid;

            //Updating fields. 
            //Note that it is necessary to create a new list of metadata fields
            //as follows and assign that list to the field list of dst. Simply emptying the field
            //list of dst and inserting fields into it would not work because such operation will
            //not utilize the overridden get and set methods.
            List<FormField> fields = new List<FormField>();
            foreach (FormFieldViewModel field in this.Fields)
                fields.Add(field.InstantiateDataModel());

            dst.Fields = fields;
        }

        private List<FormFieldType> mFieldTypes;
        public List<FormFieldType> GetFieldTypes()
        {
            if (mFieldTypes == null)
            {
                mFieldTypes = new List<FormFieldType>();
                mFieldTypes.Add(new FormFieldType());

                var fieldTypes = typeof(FormField).Assembly.GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(FormField))
                        && !t.CustomAttributes.Where(a => a.AttributeType.IsAssignableFrom(typeof(IgnoreAttribute))).Any())
                    .ToList();

                foreach (var t in fieldTypes)
                {
                    TypeLabelAttribute att = Attribute.GetCustomAttribute(t, typeof(TypeLabelAttribute)) as TypeLabelAttribute;

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
            }

            return mFieldTypes;
        }

        //Regenerate field indicies and page numbers
        public void UpdateFieldRanks()
        {
            int rank = 0;
            int page = 0;
            foreach(var field in Fields)
            {
                field.Rank = ++rank;
                field.Page = field.IsPageBreak ? ++page : page;
            }
        }
    }

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


}