using Catfish.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class TextField : BaseField
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

        public int SetValue(string val, string lang, int valueIndex = 0)
        {
            if(Values.Count <= valueIndex)
            {
                Values.Add(new MultilingualValue());
                valueIndex = Values.Count - 1;
            }
            Values[valueIndex].SetContent(val, lang);
            return valueIndex;
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
    }
}
