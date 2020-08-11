using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class MonolingualTextField : BaseField
    {
        public XmlModelList<Text> Values { get; protected set; }

        public MonolingualTextField() : base() { }
        public MonolingualTextField(XElement data) : base(data) { }
        public MonolingualTextField(string name, string desc, string lang = null) : base(name, desc, lang) { }

        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            // Populating the value list.
            Values = new XmlModelList<Text>(Data, true, Text.TagName);
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
    }
}
