using Catfish.Core.Models.Contents.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class PostAction : XmlModel
    {
        public static readonly string TagName = "post-action";
        public static readonly string LableAtt = "button-lable";
        public static readonly string FunctionAtt = "function";
        public static readonly string MessageAtt = "success-message";

        public string ButtonLabel
        {
            get => GetAttribute(LableAtt, null as string);
            set => SetAttribute(LableAtt, value);
        }

        public string Function
        {
            get => GetAttribute(FunctionAtt, null as string);
            set => SetAttribute(FunctionAtt, value);
        }
        public string SuccessMessage
        {
            get => GetAttribute(MessageAtt, null as string);
            set => SetAttribute(MessageAtt, value);
        }

        public bool ValidateInputs
        {
            get => GetAttribute("validate-inputs", true);
            set => SetAttribute("validate-inputs", value);
        }

        public XmlModelList<Mapping> StateMappings { get; set; }
        public XmlModelList<PopUp> PopUps { get; set; }
        public XmlModelList<TriggerRef> TriggerRefs { get; set; }
        public PostAction(XElement data)
            : base(data)
        {

        }
        public PostAction()
            : base(new XElement(TagName))
        {

        }
        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            //Initializing the state mappings list
            XElement stateMappingListDefinition = GetElement("state-mappings", true);
            StateMappings = new XmlModelList<Mapping>(stateMappingListDefinition, true, "mapping");

            //Initializing the popup list
            XElement popUpListDefinition = GetElement("pop-ups", true);
            PopUps = new XmlModelList<PopUp>(popUpListDefinition, true, "pop-up");

            //Initializing the triggers list
            XElement triggerRefListDefinition = GetElement("trigger-refs", true);
            TriggerRefs = new XmlModelList<TriggerRef>(triggerRefListDefinition, true, "trigger-ref");

        }

        public Mapping GetMapping(Guid current, Guid next)
        {
            return StateMappings.Where(m => m.Current == current && m.Next == next).FirstOrDefault();
        }

        ////string EncodeStateMappingCondition(Guid dataItemId, Guid controlOptionFieldId, Guid optionId)
        ////{
        ////    return string.Format("{0}:{1}:{2}", dataItemId.ToString(), controlOptionFieldId.ToString(), optionId.ToString());
        ////}
        ////void DecodeStateMappingCondition(string condition, out Guid dataItemId, out Guid controlOptionFieldId, out Guid optionId)
        ////{
        ////    string[] components = condition.Split(":");
        ////    dataItemId = Guid.Parse(components[0].Trim());
        ////    controlOptionFieldId = Guid.Parse(components[1].Trim());
        ////    optionId = Guid.Parse(components[2].Trim());
        ////}

        ////public Mapping GetMapping(Guid currentStateId, Guid dataItemId, Guid controlOptionFieldId, Guid optionId)
        ////{
        ////    string condition = EncodeStateMappingCondition(dataItemId, controlOptionFieldId, optionId);
        ////    return StateMappings.Where(m => m.Current == currentStateId && m.Condition == condition).FirstOrDefault();
        ////}


        
        public Mapping AddStateMapping(Guid current, Guid next, string button)
        {
            if (GetMapping(current, next) != null)
                throw new Exception(string.Format("Post action {0}=>{1} already exists.", current, next));

            Mapping newStateMaiipng = new Mapping() { Current = current, Next = next, ButtonLabel = button };
            StateMappings.Add(newStateMaiipng);
            return newStateMaiipng;
        }

        public Mapping AddStateMapping(Guid currentStateId, Guid nextStateId, string buttonLabel, OptionsField controlOptionFieldId, Option optionValue)
        {
            ////if (GetMapping(currentStateId, dataItemId, controlOptionFieldId, optionId) != null)
            ////    throw new Exception(string.Format("Post action already exists."));

            string function = "";
            if (typeof(RadioField).IsAssignableFrom(controlOptionFieldId.GetType()))
                function = "RadioValue";
            else if (typeof(SelectField).IsAssignableFrom(controlOptionFieldId.GetType()))
                function = "StrValue";
            //else if (typeof(CheckboxField).IsAssignableFrom(controlOptionFieldId.GetType()))
            //    function = "CheckboxValue";
            else
                throw new Exception("Unknown option filed.");

            string condition = string.Format("{0}('{1}') === '{2}'", function, controlOptionFieldId.Id.ToString(), optionValue.Id.ToString());
            Mapping newStateMaiipng = new Mapping() { Current = currentStateId, Next = nextStateId, ButtonLabel = buttonLabel, Condition = condition };
            StateMappings.Add(newStateMaiipng);
            return newStateMaiipng;
        }

        public PopUp AddPopUp(string title, string body, string message)
        {
            if (PopUps.FindByAttribute(PopUp.BodyAtt, body) != null)
                throw new Exception(string.Format("Pop-up {0} already exists.", body));

            PopUp newPopUp = new PopUp() { Title = title,Body =body, Message = message };
            PopUps.Add(newPopUp);
            return newPopUp;
        }
        public TriggerRef AddTriggerRefs(string order, Guid refId, string exceptionMessage)
        {
            if (TriggerRefs.FindByAttribute(TriggerRef.RefIdAtt, refId.ToString()) != null)
                throw new Exception(string.Format("Trigger-Ref {0}: {1} already exists.", refId.ToString(), exceptionMessage));

            TriggerRef newTriggerRef = new TriggerRef() { Order = order, RefId = refId };
            TriggerRefs.Add(newTriggerRef);
            return newTriggerRef;
        }
        public TriggerRef AddTriggerRefs(string order, Guid refId, string exceptionMessage, Guid nextStatus, bool condition)
        {
            if (TriggerRefs.FindByAttribute(TriggerRef.RefIdAtt, refId.ToString()) != null)
                throw new Exception(string.Format("Trigger-Ref {0}: {1} already exists.", refId.ToString(), exceptionMessage));

            TriggerRef newTriggerRef = new TriggerRef() { Order = order, RefId = refId, Condition = condition, NextStatus = nextStatus };
            TriggerRefs.Add(newTriggerRef);
            return newTriggerRef;
        }

    }
}
