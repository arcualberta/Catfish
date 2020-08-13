using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class PostAction : XmlModel
    {
        public static readonly string TagName = "post-action";
        public static readonly string LableAtt = "button-lable";
        public static readonly string FunctionAtt = "function";
        
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
            PopUps = new XmlModelList<PopUp>(popUpListDefinition, true, "button");

            //Initializing the triggers list
            XElement triggerRefListDefinition = GetElement("trigger-refs", true);
            TriggerRefs = new XmlModelList<TriggerRef>(triggerRefListDefinition, true, "trigger-ref");

        }
        public Mapping AddStateMapping(string current, string next)
        {
            if (StateMappings.FindByAttribute(Mapping.CurrentAtt, current) != null)
                throw new Exception(string.Format("Post action {0} already exists.", current));

            Mapping newStateMaiipng = new Mapping() { Current = current, Next = next };
            StateMappings.Add(newStateMaiipng);
            return newStateMaiipng;
        }
        public PopUp AddPopUp(string title, string message)
        {
            if (PopUps.FindByAttribute(PopUp.TitleAtt, title) != null)
                throw new Exception(string.Format("Pop-up {0} already exists.", title));

            PopUp newPopUp = new PopUp() { Title = title, Message = message };
            PopUps.Add(newPopUp);
            return newPopUp;
        }
        public TriggerRef AddTriggerRefs(string order, Guid refId)
        {
            if (TriggerRefs.FindByAttribute(TriggerRef.RefIdAtt, refId.ToString()) != null)
                throw new Exception(string.Format("Trigger-Ref {0} already exists.", refId));

            TriggerRef newTriggerRef = new TriggerRef() { Order = order, RefId = refId };
            TriggerRefs.Add(newTriggerRef);
            return newTriggerRef;
        }
        
    }
}
