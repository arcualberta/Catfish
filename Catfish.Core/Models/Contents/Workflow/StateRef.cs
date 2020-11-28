using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class StateRef : WorkflowReferrence
    {
        public static readonly string TagName = "state-ref";

        public XmlModelList<RoleReference> AuthorizedRoles { get; set; }
        public XmlModelList<EmailDomain> AuthorizedDomains { get; set; }

        public StateRef(XElement data)
            : base(data)
        {

        }
        public StateRef()
            : base(new XElement(TagName))
        {

        }


        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            //Initializing the authorization lists
            XElement authorizationsListDefinition = GetElement("authorizations", true);
            AuthorizedRoles = new XmlModelList<RoleReference>(authorizationsListDefinition, true, RoleReference.TagName);
            AuthorizedDomains = new XmlModelList<EmailDomain>(authorizationsListDefinition, true, EmailDomain.TagName);
        }
    }
}
