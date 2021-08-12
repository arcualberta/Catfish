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

        public T AppendField<T>(int? foreignId, string fieldName, string fieldDescription = null, bool isRequired = false) where T:Field, new()
        {
            T field = new T()
            {
                ForeignId = foreignId,
                Name = fieldName,
                Description = fieldDescription,
                IsRequired = isRequired,
            };
            Fields.Add(field);
            return field;
        }

        public Field AppendField<T>(int? foreignId, string fieldName, string[] options, string fieldDescription = null, bool isRequired = false) where T : OptionField, new()
        {
            OptionField field = new T()
            {
                ForeignId = foreignId,
                Name = fieldName,
                Description = fieldDescription,
                IsRequired = isRequired
            }
            .AppendOptions(options);

            Fields.Add(field);
            return field;
        }


        protected BaseField CreateDataFieldFor(Field viewField, Core.Models.Contents.Form dataModel)
        {
            string lang = null;
            BaseField dataField = null;
            switch (viewField.ComponentType)
            {
                case "LongText":
                    dataField = dataModel.CreateField<TextArea>(viewField.Name, lang, viewField.IsRequired).SetDescription(viewField.Description, lang);
                    break;
                case "EmailAddress":
                    dataField = dataModel.CreateField<EmailField>(viewField.Name, lang, viewField.IsRequired).SetDescription(viewField.Description, lang);
                    break;
                case "NumberField":
                    dataField = dataModel.CreateField<IntegerField>(viewField.Name, lang, viewField.IsRequired).SetDescription(viewField.Description, lang);
                    break;
                case "RadioButtonSet":
                    dataField = dataModel.CreateField<RadioField>(viewField.Name, lang, Array.Empty<string>(), viewField.IsRequired).SetDescription(viewField.Description, lang);
                    (viewField as OptionField).UpdateDataField(dataField);
                    break;
                case "CheckBoxSet":
                    dataField = dataModel.CreateField<CheckboxField>(viewField.Name, lang, Array.Empty<string>(), viewField.IsRequired).SetDescription(viewField.Description, lang);
                    (viewField as OptionField).UpdateDataField(dataField);
                    break;
                case "DropDownField":
                    dataField = dataModel.CreateField<SelectField>(viewField.Name, lang, Array.Empty<string>(), viewField.IsRequired).SetDescription(viewField.Description, lang);
                    (viewField as OptionField).UpdateDataField(dataField);
                    break;
                default://shortText
                    dataField = dataModel.CreateField<TextField>(viewField.Name, lang, viewField.IsRequired).SetDescription(viewField.Description, lang);
                    break;
            }

            dataField.Id = viewField.Id;
            return dataField;
        }

        public Core.Models.Contents.Form CreateDataModel()
        {
            Core.Models.Contents.Form _form = new Core.Models.Contents.Form();
            string lang = "en";

            _form.Initialize(Core.Models.XmlModel.eGuidOption.Ensure);
            _form.Id = Id; //

            _form.SetName(Name, lang);
            _form.SetDescription(Description, lang);
            foreach (Field fld in Fields)
                CreateDataFieldFor(fld, _form);

            return _form;
        }

        public void UpdateDataModel(Core.Models.Contents.Form dataModel)
        {
            string lang = "en";

            dataModel.Name.SetContent(Name, lang);
            dataModel.Description.SetContent(Description, lang);

            //Iterating over the fields in the view model, finding corresponding fields in the data model and updating them. Creates new fields in the data
            //model if no field is found with a matching ID.
            foreach(var viewField in Fields)
            {
                var dataField = dataModel.GetField(viewField.Id);
                if (dataField == null)
                    CreateDataFieldFor(viewField, dataModel);
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

            Name = dataModel.GetName(lang);//dataModel.Name.GetContent(lang);
            Description = dataModel.GetDescription(lang); //dataModel.Description.GetContent(lang);
            Random rdm = new Random();
            int fidSeed = rdm.Next(1, 1000);
            foreach (BaseField fd in dataModel.Fields)
                {
                    List<string> optionTexts = new List<string>();
                    
                
                    switch (fd.GetType().Name)
                    {
                        case "TextArea":
                             AppendField<LongText>(++fidSeed, fd.GetName(), "", fd.Required);
                            break;
                        case "EmailField":
                             AppendField<EmailAddress>(++fidSeed, fd.GetName(), "Enter a valid email address to send receipts and correspondence.", fd.Required);
                            break;
                        case "IntegerField": 
                        case "DecimalField":
                            AppendField<NumberField>(++fidSeed, fd.GetName(), "", fd.Required);
                            break;
                        case "RadioField":
                        optionTexts.Clear();
                        foreach (var op in ((Core.Models.Contents.Fields.OptionsField)fd).Options)
                                optionTexts.Add(op.OptionText.Values[0].Value);

                            AppendField<RadioButtonSet>(++fidSeed, fd.GetName(), optionTexts.ToArray(), fd.GetDescription("en"), fd.Required);
                            break;
                        case "CheckboxField":
                           
                            optionTexts.Clear();
                            foreach (var op in ((Core.Models.Contents.Fields.OptionsField)fd).Options)
                                optionTexts.Add(op.OptionText.Values[0].Value);

                            AppendField<CheckboxSet>(++fidSeed, fd.GetName(), optionTexts.ToArray(), fd.GetDescription("en"), fd.Required);
                           
                            break;
                        case "SelectField":
                            optionTexts.Clear();
                            foreach (var op in ((Core.Models.Contents.Fields.OptionsField)fd).Options)
                                optionTexts.Add(op.OptionText.Values[0].Value);

                            AppendField<DropDownMenu>(++fidSeed, fd.GetName(), optionTexts.ToArray(), fd.GetDescription("en"), fd.Required);
                            break;
                        default: //TextField -- short text 
                            AppendField<ShortText>(++fidSeed, fd.GetName(),"", fd.Required);
                            break;
                    }
                }

        }


    }
}