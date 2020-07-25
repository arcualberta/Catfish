using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class Workflow : XmlModel
    {
        public XmlModelList<State> States { get; set; }
        public XmlModelList<WorkflowAction> Actions { get; set; }

        public Workflow(XElement data)
            : base(data)
        {
        }

        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            //Initializing the States list
            XElement stateListDefinition = GetElement("states", true);
            States = new XmlModelList<State>(stateListDefinition, true, "state");

            //Initializing the actions list
            XElement actionListDefinition = GetElement("actions", true);
            Actions = new XmlModelList<WorkflowAction>(actionListDefinition, true, "action");

        }

    }
}
