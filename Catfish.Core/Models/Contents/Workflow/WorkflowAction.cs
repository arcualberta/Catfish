using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class WorkflowAction : XmlModel
    {
        public XmlModelList<Authorization> Authorizations { get; set; }
        public XmlModelList<PostAction> PostActions { get; set; }
        public WorkflowAction(XElement data)
            : base(data)
        {

        }

        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            //Initializing the authorizations list
            XElement postactionsListDefinition = GetElement("post-actions", true);
            PostActions = new XmlModelList<PostAction>(postactionsListDefinition, true, "action");

            //Initializing the authorizations list
            XElement authorizationsListDefinition = GetElement("authorizations", true);
            Authorizations = new XmlModelList<Authorization>(authorizationsListDefinition, true, "role");

        }

    }
}
