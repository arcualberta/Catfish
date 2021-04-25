using Catfish.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class TextField : BaseField, IValueField
    {
        public static readonly string ValuesTag = "values";
        public TextField() { DisplayLabel = "Short Text"; }
        public TextField(XElement data) : base(data) { DisplayLabel = "Short Text"; }
        public TextField(string name, string desc, string lang = null) : base(name, desc, lang) { DisplayLabel = "Short Text"; }


        public XmlModelList<MultilingualValue> Values { get; set; }
        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            //Building the values
            XmlModel xml = new XmlModel(Data);
            Values = new XmlModelList<MultilingualValue>(xml.GetElement(ValuesTag, true), true, MultilingualValue.TagName);
        }

        public override void SetValue(string value, string lang)
        {
            SetValue(value, lang, 0);
        }

        public int SetValue(string val, string lang, int valueIndex)
        {
            if(Values.Count <= valueIndex)
            {
                Values.Add(new MultilingualValue());
                valueIndex = Values.Count - 1;
            }
            Values[valueIndex].SetContent(val, lang);
            return valueIndex;
        }
        public void UpdateValue(Guid txtId, string value, string lang)
        {
            foreach (var multilingualVal in Values)
            {
                if (multilingualVal.Values.Any(txt => txt.Id == txtId))
                {
                    multilingualVal.UpdateValue(txtId, value, lang);
                    break;
                }
            }
        }

        public string GetValue(string lang, int valueIndex = 0)
        {
            return (Values.Count <= valueIndex) ? null : Values[valueIndex].GetContent(lang);
        }

        public override void UpdateValues(BaseField srcField)
        {
            TextField src = srcField as TextField;
            if (src == null)
                throw new Exception("The source field is null or is not a TextField");

            //Updating existing values and removing deleted values
            List<MultilingualValue> toBeDeletedValues = new List<MultilingualValue>();
            foreach (var dstMultilingualVal in Values)
            {
                var srcMultilungualVal = src.Values.Where(v => v.Id == dstMultilingualVal.Id).FirstOrDefault();

                if (srcMultilungualVal == null)
                    toBeDeletedValues.Add(dstMultilingualVal);
                else
                    dstMultilingualVal.UpdateValues(srcMultilungualVal);
            }
            foreach (var toBeDeleted in toBeDeletedValues)
                Values.Remove(toBeDeleted);

            //Insering new values
            foreach (var srcMultilingualVal in src.Values)
            {
                if(!Values.Where(v => v.Id == srcMultilingualVal.Id).Any())
                {
                    var valIndex = Values.Count;
                    foreach (var txt in srcMultilingualVal.Values)
                        SetValue(txt.Value, txt.Language, valIndex);
                }
            }
        }

        public IEnumerable<Text> GetValues(string lang = null)
        {
            var vals = Values.SelectMany(val => val.Values);
            if (!string.IsNullOrEmpty(lang))
                vals = vals.Where(v => v.Language == lang);

            return vals;
        }

        public string[] GetStrValues(string lang = null)
        {
            var vals = GetValues(lang);
            return vals.Where(v => !string.IsNullOrEmpty(v.Value))
                .Select(v => v.Value)
                .ToArray();
        }


        public string GetValues(string separator, string lang = null)
        {
            IEnumerable<Text> texts = GetValues(lang);
            return string.Join(separator, texts.Select(txt => txt.Value));
        }

        public override void CopyValue(BaseField srcField, bool overwrite = false)
        {
            var src = srcField as TextField;

            //This method is applicable only if the target field has no values in it
            bool targetFieldHasData = false;
            foreach(var val in Values)
            {
                targetFieldHasData = val.Values.Where(txt => !string.IsNullOrEmpty(txt.Value)).Any();
                if (targetFieldHasData)
                    break;
            }

            if (overwrite == false && targetFieldHasData)
                return; //We don't want to overwrite data in the target field.

            int i = 0;
            foreach (var srcMultilingualVal in src.Values)
            {
                if (!srcMultilingualVal.Values.Where(txt => !string.IsNullOrEmpty(txt.Value)).Any())
                    continue; //No data is available under any of the languages

                if (Values.Count <= i)
                    Values.Add(new MultilingualValue());

                var dstMultilingualVal = Values[i];

                foreach(var srcTxt in srcMultilingualVal.Values)
                {
                    var dstTxt = dstMultilingualVal.Values.Where(txt => txt.Language == srcTxt.Language).FirstOrDefault();

                    if (dstTxt == null)
                        srcMultilingualVal.Values.Add(new Text(srcTxt));
                    else
                        dstTxt.Copy(srcTxt);
                }
            }
        }
    }
}
