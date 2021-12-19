using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class MonolingualTextField : BaseField, IValueField
    {
        public XmlModelList<Text> Values { get; protected set; }

        public MonolingualTextField() : base() { DisplayLabel = "Multilingual Text"; }
        public MonolingualTextField(XElement data) : base(data) { DisplayLabel = "Multilingual Text"; }
        public MonolingualTextField(string name, string desc, string lang = null) : base(name, desc, lang) { DisplayLabel = "Multilingual Text"; }

        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            // Populating the value list.
            Values = new XmlModelList<Text>(Data, true, Text.TagName);
        }

        public override void SetValue(string value, string lang)
        {
            SetValue(value, 0);
        }

        /// <summary>
        /// Sets the value at the given index
        /// </summary>
        /// <param name="val"></param>
        /// <param name="valueIndex"></param>
        /// <returns></returns>
        public int SetValue(object val, int valueIndex = 0)
        {
            if (Values.Count <= valueIndex)
            {
                Values.Add(new Text());
                valueIndex = Values.Count - 1;
            }
            Values[valueIndex].Value = val == null ? "" : val.ToString();
            return valueIndex;
        }

        /// <summary>
        /// Get the value at the given index
        /// </summary>
        /// <param name="valueIndex"></param>
        /// <returns></returns>
        public string GetValue(int valueIndex = 0)
        {
            return Values[valueIndex].Value;
        }

        public string[] GetValues()
        {
            return Values.Where(v => !string.IsNullOrEmpty(v.Value))
                .Select(v => v.Value)
                .ToArray();
        }

        public override void UpdateValues(BaseField srcField)
        {
            MonolingualTextField src = srcField as MonolingualTextField;
            if (src == null)
                throw new Exception("The source field is null or is not a MonolingualTextField");

            //Updating existing values and removing deleted values
            List<Text> toBeDeletedValues = new List<Text>();
            foreach (var dstTextVal in Values)
            {
                var srcTextVal = src.Values.Find(dstTextVal.Id);

                if (srcTextVal == null)
                    toBeDeletedValues.Add(dstTextVal);
                else
                    dstTextVal.Value = srcTextVal.Value;
            }
            foreach (var toBeDeleted in toBeDeletedValues)
                Values.Remove(toBeDeleted);

            //Insering new values
            foreach (var srcVal in src.Values)
            {
                if (Values.Find(srcVal.Id) == null)
                    Values.Add(new Text(new XElement(srcVal.Data)));
            }
        }

        public void UpdateValues(BaseField srcField, string textVal = null)
        {
            MonolingualTextField src = srcField as MonolingualTextField;
            if (src == null)
                throw new Exception("The source field is null or is not a MonolingualTextField");

            //Updating existing values and removing deleted values
            List<Text> toBeDeletedValues = new List<Text>();
            foreach (var dstTextVal in Values)
            {
                var srcTextVal = src.Values.Find(dstTextVal.Id);

                if (srcTextVal == null && textVal == null)
                    toBeDeletedValues.Add(dstTextVal);
                else
                    dstTextVal.Value = srcTextVal != null ? srcTextVal.Value : textVal;
            }
            foreach (var toBeDeleted in toBeDeletedValues)
                Values.Remove(toBeDeleted);

            //Insering new values
            foreach (var srcVal in src.Values)
            {
                if (Values.Find(srcVal.Id) == null)
                    Values.Add(new Text(new XElement(srcVal.Data)));
            }
        }

        public void UpdateValue(Guid txtId, string value, string lang)
        {
            var txt = Values.Where(txt => txt.Id == txtId).FirstOrDefault();
            if (txt != null)
                txt.Value = value;
            else
                Values.Add(new Text() { Value = value, Language = lang });
        }

        public IEnumerable<Text> GetValues(string lang = null)
        {
            return Values;
        }

        public virtual string GetValues(string separator, string lang = null)
        {
            return string.Join(separator, Values.Select(txt => txt.Value));
        }

        public override void CopyValue(BaseField srcField, bool overwrite = false)
        {
            var src = srcField as MonolingualTextField;

            if (overwrite || Values.Where(txt => !string.IsNullOrEmpty(txt.Value)).Any() == false)
            {
                var srcValues = src.Values.Where(txt => !string.IsNullOrEmpty(txt.Value)).ToList();

                //Iterate through every value in the src field and set or inser them to the target field
                for(int i=0; i< srcValues.Count; ++i)
                {
                    if (i < Values.Count)
                        Values[i].Copy(srcValues[i]);
                    else
                        Values.Add(new Text(srcValues[i]));
                }
            }
        }
    }
}
