using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class Workflow : XmlModel
    {
        public XmlModelList<State> States { get; set; }
        public XmlModelList<WorkflowAction> Actions { get; set; }
        public XmlModelList<Authorization> Authorizations { get; set; }

        public Workflow(XElement data)
            : base(data)
        {
            Initialize(eGuidOption.Ensure);
        }

        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            //Initializing the States list
            XElement stateListDefinition = Data.Element("states");
            States = new XmlModelList<State>(stateListDefinition, true, "state");

            //Initializing the actions list
            XElement actionListDefinition = Data.Element("actions");
            Actions = new XmlModelList<WorkflowAction>(actionListDefinition, true, "action");

            //Initializing the authorizations list
            XElement authorizationsListDefinition = Data.Element("authorizations");
            Authorizations = new XmlModelList<Authorization>(authorizationsListDefinition, true, "role");

        }

    }
}
