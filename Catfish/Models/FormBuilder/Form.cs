using Catfish.Core.Models.Contents.Fields;
using Catfish.Models.FormBuilder.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catfish.Models.FormBuilder
{
    public class Form
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int? ForeignId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LinkText { get; set; }
        public List<Field> Fields { get; set; } = new List<Field>();

        public T AppendField<T>(int? foreignId, string fieldName, string fieldDescription = null, bool isRequired = false, Guid? id=null) where T:Field, new()
        {
            T field = new T()
            {
                ForeignId = foreignId,
                Name = fieldName,
                Description = fieldDescription,
                IsRequired = isRequired     
            };

            if (id != null)
                field.Id = id.Value;


            Fields.Add(field);
            return field;
        }

        public Field AppendField<T>(int? foreignId, string fieldName, string[] options, string fieldDescription = null, bool isRequired = false, Guid? id=null, bool[] extendedOptions=null) where T : OptionField, new()
        {
            OptionField field = new T()
            {
                ForeignId = foreignId,
                Name = fieldName,
                Description = fieldDescription,
                IsRequired = isRequired
            }
            .AppendOptions(options,extendedOptions);

            if (id != null)
                field.Id = id.Value;

            Fields.Add(field);
            return field;
        }

        public Core.Models.Contents.Form CreateDataModel()
        {
            Core.Models.Contents.Form _form = new Core.Models.Contents.Form();

            _form.Initialize(Core.Models.XmlModel.eGuidOption.Ensure);
            _form.Id = Id;
            UpdateDataModel(_form);

            return _form;
        }

        public void UpdateDataModel(Core.Models.Contents.Form dataModel)
        {
            string lang = "en";

            dataModel.FormName = Name;
            dataModel.Name.SetContent(Name, lang);
            dataModel.Description.SetContent(Description, lang);

            //Iterating over the fields in the view model, finding corresponding fields in the data model and updating them. Creates new fields in the data
            //model if no field is found with a matching ID.
            foreach(var viewField in Fields)
            {
                var dataField = dataModel.GetField(viewField.Id);
                if (dataField == null)
                    viewField.CreateDataFieldFor(dataModel);
                else
                    viewField.UpdateDataField(dataField);
            }

            //Removing fields from the data model if there is no field in the view model with the same ID
            var viewFieldIds = Fields.Select(f => f.Id).ToList();
            var toBeDeleted = dataModel.Fields.Where(f => !viewFieldIds.Contains(f.Id)).ToList();
            foreach (var item in toBeDeleted)
                dataModel.Fields.Remove(item);
        }

        public void UpdateViewModel(Core.Models.Contents.FieldContainer dataModel)//(Core.Models.Contents.Form dataModel)
        {
            string lang = "en";

            Id = dataModel.Id; //set the form id with existing form id
            Name = dataModel.GetName(lang);
            Description = dataModel.GetDescription(lang);
            Random rdm = new Random();
            int fidSeed = rdm.Next(1, 1000);
            foreach (BaseField fd in dataModel.Fields)
            {
                List<string> optionTexts = new List<string>();
                List<bool> extendedOptions = new List<bool>();

                switch (fd.GetType().Name)
                {
                    case "TextArea":
                        AppendField<LongText>(++fidSeed, fd.GetName(), "", fd.Required, fd.Id);
                        break;
                    case "EmailField":
                        AppendField<EmailAddress>(++fidSeed, fd.GetName(), "Enter a valid email address to send receipts and correspondence.", fd.Required, fd.Id);
                        break;
                    case "IntegerField":
                    case "DecimalField":
                        AppendField<NumberField>(++fidSeed, fd.GetName(), "", fd.Required, fd.Id);
                        break;
                    case "RadioField":
                        optionTexts.Clear();
                        extendedOptions.Clear();
                        foreach (var op in ((Core.Models.Contents.Fields.OptionsField)fd).Options)
                        {
                            optionTexts.Add(op.OptionText.Values[0].Value);
                            extendedOptions.Add(op.ExtendedOption);
                        }
                        AppendField<RadioButtonSet>(++fidSeed, fd.GetName(), optionTexts.ToArray(), fd.GetDescription("en"), fd.Required, fd.Id, extendedOptions.ToArray());
                        break;
                    case "CheckboxField":

                        optionTexts.Clear();
                        extendedOptions.Clear();
                        foreach (var op in ((Core.Models.Contents.Fields.OptionsField)fd).Options)
                        {
                            optionTexts.Add(op.OptionText.Values[0].Value);
                            extendedOptions.Add(op.ExtendedOption);
                        }

                        AppendField<CheckboxSet>(++fidSeed, fd.GetName(), optionTexts.ToArray(), fd.GetDescription("en"), fd.Required, fd.Id, extendedOptions.ToArray());

                        break;
                    case "SelectField":
                        optionTexts.Clear();
                        foreach (var op in ((Core.Models.Contents.Fields.OptionsField)fd).Options)
                            optionTexts.Add(op.OptionText.Values[0].Value);

                        AppendField<DropDownMenu>(++fidSeed, fd.GetName(), optionTexts.ToArray(), fd.GetDescription("en"), fd.Required, fd.Id);
                        break;
                    default: //TextField -- short text 
                        AppendField<ShortText>(++fidSeed, fd.GetName(), "", fd.Required, fd.Id);
                        break;
                }
            }

        }


    }
}