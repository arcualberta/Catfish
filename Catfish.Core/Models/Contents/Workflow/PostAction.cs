using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class PostAction : XmlModel
    {
        public string Function { get; set; }
        public string ButtonLabel { get; set; }
        public XmlModelList<StateMapping> StateMappings { get; set; }
        public XmlModelList<PopUp> PopUps { get; set; }
        public XmlModelList<Trigger> Triggers { get; set; }
        public PostAction(XElement data)
            : base(data)
        {

        }
        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            //Initializing the state mappings list
            XElement stateMappingListDefinition = GetElement("state-mappings", true);
            StateMappings = new XmlModelList<StateMapping>(stateMappingListDefinition, true, "map");

            //Initializing the popup list
            XElement popUpListDefinition = GetElement("pop-up", true);
            PopUps = new XmlModelList<PopUp>(popUpListDefinition, true, "button");

            //Initializing the triggers list
            XElement triggerListDefinition = GetElement("triggers", true);
            Triggers = new XmlModelList<Trigger>(triggerListDefinition, true, "trigger");

        }
    }
}
