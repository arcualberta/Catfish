using Catfish.Core.Helpers;
using Catfish.Core.Models.Contents.Expressions;
using Catfish.Core.Models.Contents.Workflow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public abstract class BaseField : XmlModel
    {
        public const string FieldTagName = "field";

        public string DisplayLabel { get; set; }

        //public virtual void UpdateValues(BaseField srcField) 
        //{
        //    throw new Exception("This method must be overridden by sub classes");
        //}
        public abstract void UpdateValues(BaseField srcField);
        public abstract void SetValue(string values, string lang);
        public abstract void CopyValue(BaseField srcField, bool overwrite = false);

        public bool Required
        {
            get => GetAttribute("required", false); 
            set => SetAttribute("required", value);
        }

        public bool AllowMultipleValues
        {
            get => GetAttribute("multiple", false);
            set => SetAttribute("multiple", value);
        }

        public bool Readonly
        {
            get => GetAttribute("readonly", false);
            set => SetAttribute("readonly", value);
        }

        public Guid? RefId
        {
            get => GetAttribute("ref-id", null as Guid?);
            set => SetAttribute("ref-id", value);
        }

        public bool IsListEntryTitle
        {
            get => GetAttribute("list-entry-title", false);
            set => SetAttribute("list-entry-title", value);
        }

        public bool IsListEntryDescription
        {
            get => GetAttribute("list-entry-desc", false);
            set => SetAttribute("list-entry-desc", value);
        }

        ///Request to exclude the user-input field from rendering. However, the remaining
        ///properties such as ID and model-type will be rendered.
        public bool Exclude
        {
            get => GetAttribute("exlude", false);
            set { SetAttribute("exlude", value); }
        }

        public MultilingualName Name { get; protected set; }

        public MultilingualDescription Description { get; protected set; }

        public string VueComponent => GetType().FullName;

        private VisibilityCondition mVisibilityCondition;
        public VisibilityCondition VisibilityCondition { get { if(mVisibilityCondition == null) mVisibilityCondition = new VisibilityCondition(GetElement(VisibilityCondition.TagName, true)); return mVisibilityCondition; } }
        
        private RequiredCondition mRequiredCondition;
        public RequiredCondition RequiredCondition { get { if(mRequiredCondition == null) mRequiredCondition = new RequiredCondition(GetElement(RequiredCondition.TagName, true)); return mRequiredCondition; } }

        private ValueExpression mValueExpression;
        public ValueExpression ValueExpression { get { if (mValueExpression == null) mValueExpression = new ValueExpression(GetElement(ValueExpression.TagName, true)); return mValueExpression; } }
        public bool HasValueExpression { get => mValueExpression != null || GetElement(ValueExpression.TagName, false) != null; }

        private FieldReference mSourceFieldReference;

        public BaseField() : base(FieldTagName) { }
        public BaseField(XElement data) : base(data) { }

        public BaseField(string name, string desc, string lang = null)
            : base(FieldTagName)
        {
            //Name = new MultilingualText(GetElement(Entity.NameTag, true));
            Name.SetContent(name, lang);

            //Description = new MultilingualText(GetElement("description", true));
            Description.SetContent(desc, lang);
        }

        public override void Initialize(eGuidOption guidOption)
        {
            //Ensuring that each field has a unique ID
            base.Initialize(guidOption == eGuidOption.Ignore ? eGuidOption.Ensure : guidOption);

            Name = new MultilingualName(GetElement(MultilingualName.TagName, true));
            Description = new MultilingualDescription(GetElement(MultilingualDescription.TagName, true));

            var sourceReference = GetElement(FieldReference.TagName, false);
            if (sourceReference != null)
                mSourceFieldReference = new FieldReference(sourceReference);
        }

        public FieldReference GetSourceReference(bool createIfNotExist = false)
        {
            if (mSourceFieldReference == null && createIfNotExist)
                mSourceFieldReference = new FieldReference(GetElement(FieldReference.TagName, true));
            return mSourceFieldReference;
        }

        public string GetName()
        {
            return Name.GetConcatenatedContent(" | ");
        }

        public string GetName(string lang)
        {
            Text val = Name.Values.Where(val => val.Language == lang).FirstOrDefault();
            return val != null ? val.Value : null;
        }

        public void SetName(string containerName, string lang)
        {
            Name.SetContent(containerName, lang);
        }

        public string GetDescription(string lang)
        {
            Text val = Description.Values.Where(val => val.Language == lang).FirstOrDefault();
            return val != null ? val.Value : null;
        }

        public BaseField SetDescription(string containerDescription, string lang)
        {
            Description.SetContent(containerDescription, lang);
            return this;
        }

        public string FieldCssClass
        {
            get => GetAttribute("field-css", "");
            private set => Data.SetAttributeValue("field-css", value);
        }
        public BaseField SetFieldCssClass(string value) { FieldCssClass = value; return this; }
        public string FieldLabelCssClass
        {
            get => GetAttribute("label-css", "col-md-4");
            set => Data.SetAttributeValue("label-css", value);
        }
        public BaseField SetFieldLabelCssClass(string value) { FieldLabelCssClass = value; return this; }

        public string FieldValueCssClass
        {
            get => GetAttribute("value-css", "col-md-8");
            set => Data.SetAttributeValue("value-css", value);
        }
        public BaseField SetFieldValueCssClass(string value) { FieldValueCssClass = value; return this; }

        public eSolrFieldType SolrFieldType
        {
            get => GetAttribute<eSolrFieldType>("solr-type", eSolrFieldType._ts);
            set { SetAttribute("solr-type", value); }
        }
        public BaseField SetSolrFieldType(eSolrFieldType value) { SolrFieldType = value; return this; }


        /// <summary>
        /// char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
        /// </summary>
        /// <param name="field"></param>
        /// <param name="delimiters"></param>
        /// <param name="interestedIndex"></param>
        /// <returns></returns>
        public string ParseFieldValue(char[] delimiters = null, int interestedIndex=-1)
        {
            string strVal = "";

            if (delimiters != null)
            {
                string[] parsedString = null;
                if (typeof(Catfish.Core.Models.Contents.Fields.TextField).IsAssignableFrom(this.GetType()))
                {
                    parsedString = ((TextField)this).GetValue(null).Split(delimiters);
                }
                else if (typeof(Catfish.Core.Models.Contents.Fields.OptionsField).IsAssignableFrom(this.GetType()))
                {
                   // this only good for single selected option field
                   foreach(Option op in ((OptionsField)this).Options)
                    {
                        if (op.Selected)
                        {
                            parsedString = op.OptionText.Values[0].Value.Split(delimiters);
                            break;
                        }

                    }
                }

                //get the interested string
                if (interestedIndex >= 0 && interestedIndex <= parsedString.Length)
                {
                    strVal = parsedString[interestedIndex].Trim();
                }
                else
                {
                    strVal = ((TextField)this).GetValue(null);
                }
            }     
         return strVal;
        }

      

        /// <summary>
        /// Set the value based on the FieldId reference: ie: if a text box should be prefilled based on selected value or part of selected value from previous Field
        /// </summary>
        /// <returns></returns>
        public BaseField SetDefaultReferenceValue(BaseField refField,string delimiter=null, int interestedIndex=-1 /* index after parsing*/)
        {
            string refValue = "";

            if (delimiter != null)
                refValue = refField.Id + ":[" + delimiter + "," + interestedIndex + "]";
            else
                refValue = refField.Id.ToString();

            this.SetAttribute("default-val-ref", refValue);
           
            return this;
        }

    }
}
