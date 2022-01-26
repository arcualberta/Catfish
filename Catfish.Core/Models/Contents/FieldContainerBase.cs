using Catfish.Core.Models.Contents.Fields;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents
{
    public class FieldContainerBase : XmlModel
    {
        public const string FieldContainerTag = "fields";

        [NotMapped]
        public FieldList Fields { get; protected set; }

        public FieldContainerBase(string tagName) : base(tagName)
        {
            Initialize(eGuidOption.Ignore);
            Created = DateTime.Now;
        }
        public FieldContainerBase(XElement data) : base(data)
        {
            Initialize(eGuidOption.Ignore);
        }

        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            Fields = new FieldList(GetElement(FieldContainerTag, true));
        }

        /// <summary>
        /// Returns the Field of which the name is the name given by the input argument fieldName in the language specified by
        /// the input argument lang
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public BaseField GetFieldByName<T>(string fieldName, string lang)
            where T : BaseField
        {
            var fieldsOfGivenType = Fields.Where(f => f is T);
            foreach (var field in fieldsOfGivenType)
            {
                if (field.Name.Values.Where(v => v.Language == lang && v.Value == fieldName).FirstOrDefault() != null)
                    return field;
            }
            return null;
        }

        public T GetField<T>(string fieldName, string fieldNameLang, bool createIfNotExists = true)
            where T : TextField
        {
            T field = GetFieldByName<T>(fieldName, fieldNameLang) as T;

            if (field == null && createIfNotExists)
            {
                if (typeof(TextArea).IsAssignableFrom(typeof(T)))
                    field = new TextArea() as T;
                else
                    field = new TextField() as T;

                field.Name.SetContent(fieldName, fieldNameLang);
                Fields.Add(field);
            }

            return field;
        }

        public BaseField GetField(Guid fieldId)
        {
            return Fields.FirstOrDefault(f => f.Id == fieldId);
        }

        public T CreateField<T>(string fieldName, string lang, bool? isRequired = null, bool? allowMultiple = null, object defaultValue = null)
            where T : MonolingualTextField
        {
            T field = Activator.CreateInstance(typeof(T)) as T;

            Fields.Add(field);
            field.SetName(fieldName, lang);

            if (isRequired.HasValue)
                field.Required = isRequired.Value;

            if (allowMultiple.HasValue)
                field.AllowMultipleValues = allowMultiple.Value;

            if (defaultValue != null)
                field.SetValue(defaultValue);
            else
            {
                if (typeof(DateField).IsAssignableFrom(typeof(T)))
                    field.SetValue(new DateTime());
                else
                    field.SetValue("");
            }

            return field;
        }
        public T CreateField<T>(string fieldName, string lang, bool? isRequired = null, bool? allowMultiple = null, string defaultValue = null)
            where T : TextField
        {
            T field = Activator.CreateInstance(typeof(T)) as T;

            Fields.Add(field);
            field.SetName(fieldName, lang);

            if (isRequired.HasValue)
                field.Required = isRequired.Value;

            if (allowMultiple.HasValue)
                field.AllowMultipleValues = allowMultiple.Value;

            if (defaultValue != null)
                field.SetValue(defaultValue, lang);
            else
                field.SetValue("", lang);

            return field;
        }

        public T CreateField<T>(string fieldName, string lang, FieldContainerReference.eRefType refType, Guid? refId)
            where T : FieldContainerReference
        {
            T field = Activator.CreateInstance(typeof(T)) as T;

            Fields.Add(field);
            field.SetName(fieldName, lang);

            field.RefType = refType;
            if (refId.HasValue)
                field.RefId = refId.Value;
            return field;
        }

        public void SetFieldValue<T>(string fieldName, string fieldNameLang, string fieldValue, string fieldValueLang, bool createIfNotExists = true, int valueIndex = 0)
            where T : TextField
        {
            T field = GetField<T>(fieldName, fieldNameLang, createIfNotExists);
            field.SetValue(fieldValue, fieldValueLang, valueIndex);
        }
        public void SetFieldValue<T>(string fieldName, string fieldNameLang, string[] fieldValues, string fieldValueLang, bool createIfNotExists = true, int startValueIndex = 0)
            where T : TextField
        {
            T field = GetField<T>(fieldName, fieldNameLang, createIfNotExists);
            int valueIndex = startValueIndex;
            foreach (string val in fieldValues)
                field.SetValue(val, fieldValueLang, valueIndex);
        }

        public string GetValue<T>(string fieldName, string fieldNameLanguage, string fieldValueLanguage)
            where T : TextField
        {
            TextField field = GetField<T>(fieldName, fieldNameLanguage, false);
            return field != null ? field.GetValue(fieldValueLanguage) : null;
        }

        public List<string> GetValues<T>(string fieldName, string fieldNameLanguage, string fieldValueLanguage)
            where T : TextField
        {
            TextField field = GetField<T>(fieldName, fieldNameLanguage, false);
            List<string> values = new List<string>();
            foreach (var val in field.Values)
            {
                string v = val.Values.Where(txt => txt.Language == fieldValueLanguage).Select(txt => txt.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(v))
                    values.Add(v);
            }

            return values;
        }

        public T CreateField<T>(string fieldName, string lang, string[] optionTexts, bool? isRequired = null, int? defaultValueIndex = null)
            where T : OptionsField
        {
            T field = Activator.CreateInstance(typeof(T)) as T;

            Fields.Add(field);
            field.SetName(fieldName, lang);
            field.AddOptions(optionTexts, lang, defaultValueIndex);

            if (isRequired.HasValue)
                field.Required = isRequired.Value;

            return field;
        }

        public T CreateField<T>(string content, string lang, string cssClass = null, string fieldName = null)
            where T : InfoSection
        {
            T field = Activator.CreateInstance(typeof(T)) as T;

            Fields.Add(field);

            if (!string.IsNullOrEmpty(content))
                field.SetContent(content, lang);

            if (!string.IsNullOrEmpty(fieldName))
                field.SetName(fieldName, lang);

            if (!string.IsNullOrEmpty(cssClass))
                field.CssClass = cssClass;

            return field;
        }


        public T CreateField<T>(string fieldName, string lang, bool? isRequired = null)
            where T : CompositeField
        {
            T field = Activator.CreateInstance(typeof(T)) as T;

            Fields.Add(field);
            field.SetName(fieldName, lang);

            if (isRequired.HasValue)
                field.Required = isRequired.Value;

            return field;
        }

        public T CreateField<T>(string fieldName, string lang, bool? isRequired = null, int? maxFileSize = null)
            where T : AttachmentField
        {
            T field = Activator.CreateInstance(typeof(T)) as T;

            Fields.Add(field);
            field.SetName(fieldName, lang);

            if (isRequired.HasValue)
                field.Required = isRequired.Value;

            return field;
        }
        public T CreateField<T>(string fieldName, string lang, bool? isRequired = null, string format=null)
           where T : AudioRecordingField
        {
            T field = Activator.CreateInstance(typeof(T)) as T;

            Fields.Add(field);
            field.SetName(fieldName, lang);

            if (isRequired.HasValue)
                field.Required = isRequired.Value;

            return field;
        }
        public T CreateField<T>(string fieldName, string lang, bool? isRequired = null, int minRows = 0, int? maxRows = null)
            where T : TableField
        {
            T field = Activator.CreateInstance(typeof(T)) as T;

            Fields.Add(field);
            field.SetName(fieldName, lang);
            field.MinRows = minRows;
            if (maxRows.HasValue)
                field.MaxRows = maxRows;

            if (isRequired.HasValue)
                field.Required = isRequired.Value;

            return field;
        }

        public T CreateField<T>() where T : BaseField
        {
            T field = Activator.CreateInstance(typeof(T)) as T;
            Fields.Add(field);
            return field;
        }

        public void UpdateFieldValues(FieldContainer dataSrc)
        {
            foreach (var dst in Fields)
            {
                var srcField = dataSrc.Fields.Where(f => f.Id == dst.Id).FirstOrDefault();

                if (srcField != null)
                    dst.UpdateValues(srcField);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file">C:\somefolder\somefile.txt</param>

        /// <param name="ToDir">wwwroot/uploads/*/*</param>
        private void MoveFile(string filePath, string ToDir)
        {
            string toPath = Path.Combine(
                   Directory.GetCurrentDirectory(),
                   ToDir);

            string directory = Path.GetDirectoryName(filePath);

            foreach (string file in Directory.GetFiles(directory))
            {
                //get the filename
                string[] tempf = file.Split('\\');
                tempf = tempf[tempf.Length - 1].Split("__");
                string fn = tempf[0] + tempf[tempf.Length - 1];

                string destinationFile = ToDir + fn;

                if (File.Exists(destinationFile))
                    File.Delete(destinationFile);

                File.Move(file, destinationFile);
            }
        }
        private string CreateDirectory(string parentDirectory, string subDirectory)
        {
            //create temp directory for login user
            string directory = parentDirectory + subDirectory;
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            return directory + "/";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">field GUID id</param>
        /// <param name="fromDir">"wwwroot/uploads/temp"</param>
        /// <returns></returns>
        private string[] GetAttachmentFile(string name, string fromDir)
        {
            string path = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    fromDir);
            string[] files = Directory.GetFiles(path, "*" + name + "_*.*");
            if (files.Length > 0)
                return files;

            return null;
        }
        public BaseField GetFieldByName(string fieldName, string lang)
        {
            foreach (var field in Fields)
            {
                if (field.Name.Values.Where(v => v.Language == lang && v.Value == fieldName) != null)
                    return field;
            }
            return null;
        }

        public IList<IValueField> GetInputFields()
        {
            var inputFields = Fields.Where(f => typeof(IValueField).IsAssignableFrom(f.GetType()))
                    .Select(f => f as IValueField)
                    .ToList();

            return inputFields;
        }

        public List<string> GetValues(Guid id, string lang = null)
        {
            IValueField valField = Fields
                .Where(f => typeof(IValueField).IsAssignableFrom(f.GetType()) && f.Id == id)
                .Select(f => f as IValueField)
                .FirstOrDefault();

            return valField == null 
                ? new List<string>() 
                : valField
                    .GetValues(lang) //This is a list of Text elements, optionally filtered based on the specified language
                    .Select(txt => txt.Value) //Extract the literal text string encapsulated in the Text element
                    .ToList();

        }

        public string GetValues(Guid id, string separator, string lang = null)
        {
            IValueField valField = Fields
                .Where(f => typeof(IValueField).IsAssignableFrom(f.GetType()) && f.Id == id)
                .Select(f => f as IValueField)
                .FirstOrDefault();

            return valField == null ? null : valField.GetValues(separator, lang);
        }

        public List<BaseField> GetValueFields()
        {
            return Fields.Where(field => typeof(IValueField).IsAssignableFrom(field.GetType())).ToList();
        }

        public List<string> GetConcatenatedFieldValues(IList<Guid> fieldIds, string separator)
        {
            List<string> result = new List<string>();
            foreach (var id in fieldIds)
            {
                var dataField = Fields.Where(f => f.Id == id).FirstOrDefault() as IValueField;
                result.Add(dataField == null ? "" : (dataField as IValueField).GetValues(" | ", null));
            }
            return result;
        }
    }
}
