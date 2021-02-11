using Catfish.Core.Helpers;
using Catfish.Core.Models.Contents.Expressions;
using System;
using System.Collections.Generic;
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

        public MultilingualName Name { get; protected set; }

        public MultilingualDescription Description { get; protected set; }

        public string VueComponent => GetType().FullName;

        private VisibilityCondition mVisibilityCondition;
        public VisibilityCondition VisibilityCondition { get { if(mVisibilityCondition == null) mVisibilityCondition = new VisibilityCondition(GetElement(VisibilityCondition.TagName, true)); return mVisibilityCondition; } }
        
        private RequiredCondition mRequiredCondition;
        public RequiredCondition RequiredCondition { get { if(mRequiredCondition == null) mRequiredCondition = new RequiredCondition(GetElement(RequiredCondition.TagName, true)); return mRequiredCondition; } }

        private ValueExpression mValueExpression;
        public ValueExpression ValueExpression { get { if (mValueExpression == null) mValueExpression = new ValueExpression(GetElement(ValueExpression.TagName, true)); return mValueExpression; } }

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

        #region Visible-If
        public Guid? VisibleIfOptionFieldId
        {
            get => GetAttribute("visible-if-option-field-id", null as Guid?);
            private set => Data.SetAttributeValue("visible-if-option-field-id", value);
        }
        public Guid[] VisibleIfOptionIds
        {
            get => GetAttribute("visible-if-option-id", null as Guid[]);
            private set => Data.SetAttributeValue("visible-if-option-id", value == null ? "" : string.Join(",", value.Select(v => v.ToString())));
        }
        public BaseField SetVisibleIf(OptionsField controllerField, string triggerOptionValue)
        {
            VisibleIfOptionFieldId = controllerField.Id;
            VisibleIfOptionIds = controllerField.Options
                .Where(op => op.OptionText.Values.Select(txt => txt.Value).Contains(triggerOptionValue))
                .Select(op => op.Id)
                .ToArray();

            return this;
        }
        #endregion

        #region Option-if
      
        public Guid[] OptionIfFieldIds
        {
            get => GetAttribute("option-if-field-ids", null as Guid[]);
            private set => Data.SetAttributeValue("option-if-field-ids", value == null ? "" : string.Join(",", value.Select(v => v.ToString())));
        }

      

        public BaseField SetOptionIf(List<OptionsField> controllerFields ,string[] optionTexts=null,  string triggerOptionValues=null)
        {
            //triggerOptionValues contains "index, optionValue, operator" : index - index object in controllerFields
            //                                                              optionValue -- if this field not empty, the operator will be used to set the selected option on this field
            //
            List<Guid> _optionIfIds = new List<Guid>();
            foreach(OptionsField field in controllerFields)
            {
                _optionIfIds.Add(field.Id);
            }
            if(_optionIfIds.Count > 0)
                OptionIfFieldIds = _optionIfIds.ToArray();

            string[] triggerOptions = string.IsNullOrWhiteSpace(triggerOptionValues) ? null: triggerOptionValues.Split(',');
           
            if (triggerOptions.Count() > 0)
            {
                int index = Convert.ToInt32(triggerOptions[0].Trim());
                string triggerVal = triggerOptions[1].Trim(); //YEs - isChair -- then ignore the rest select first index which if the faculty Dean
                string logicalOperator = triggerOptions[2].Trim();

               
                foreach (var radioOpt in controllerFields.ElementAt(index).Options)
                {
                    if (radioOpt.OptionText.Values.Where(v => v.Value.Equals(triggerVal)).FirstOrDefault() == null)//yes/no
                    {
                        //isChair = No
                        //applicant is not a department chair
                        //select department cahie according to the selecteddepartment

                        string strIfCondition1 = OptionIfFieldIds.ElementAt(index) + ":" + "[" + controllerFields.ElementAt(index).Options[1].Id + "] " + logicalOperator + " "; //radio button set
                        for (int i = 0; i < controllerFields.Count; i++)
                        {
                            if (i != index)
                            {   //fieldGuid:OptionGuid
                                //get radio button yes/no
                                string strIfCondition2 = controllerFields.ElementAt(i).Id + ":" + "[";
                                int optInx = 1;//1 because options[0] visible-if have been set.
                                foreach (var option in controllerFields.ElementAt(i).Options)
                                {
                                    foreach (string text in optionTexts)
                                    {
                                        if (text.Contains(option.OptionText.Values[0].Value))
                                        {
                                            strIfCondition2 += option.Id;
                                            break;
                                        }
                                    }
                                    (this as OptionsField).Options[optInx].SetAttribute("visible-if", strIfCondition1 + strIfCondition2);
                                    optInx++;
                                   // VisibleIf = strIfCondition;
                                   strIfCondition2 = "";
                                }

                               
                            }

                        }


                    }
                    else
                    {
                        for (int i = 0; i < controllerFields.Count; i++)
                        {
                            if (i != index)
                            {
                                (this as OptionsField).Options[0].SetAttribute("visible-if", controllerFields.ElementAt(index).Id + ":[" + controllerFields.ElementAt(index).Options[0].Id + "]");
                             
                            }
                        }
                    }
                }

            }
            return this;
        }

        public BaseField SetOptionIf(OptionsField controllerField, string triggerOptionValue)
        {
            foreach (var opt in controllerField.Options)
            {
                if (opt.OptionText.Values.Where(v => v.Value.Equals(triggerOptionValue)).FirstOrDefault() != null)
                {
                    string strIfCondition1 = controllerField.Id + ":" + opt.Id; //radio button set
                    this.SetAttribute("visible-if", strIfCondition1);
                    break;
                }
            }
            return this;
        }


        #endregion

        public string FieldCssClass
        {
            get => GetAttribute("field-css", "");
            private set => Data.SetAttributeValue("field-css", value);
        }
        public BaseField SetFieldCssClass(string value) { FieldCssClass = value; return this; }
        public string FieldLabelCssClass
        {
            get => GetAttribute("label-css", "col-md-4");
            private set => Data.SetAttributeValue("label-css", value);
        }
        public BaseField SetFieldLabelCssClass(string value) { FieldLabelCssClass = value; return this; }

        public string FieldValueCssClass
        {
            get => GetAttribute("value-css", "col-md-8");
            private set => Data.SetAttributeValue("value-css", value);
        }
        public BaseField SetFieldValueCssClass(string value) { FieldValueCssClass = value; return this; }

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
