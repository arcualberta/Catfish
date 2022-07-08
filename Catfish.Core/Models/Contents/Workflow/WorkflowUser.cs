using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class WorkflowUser : XmlModel
    {
        public static readonly string TagName = "user";

        public XmlModelList<RoleReference> Roles { get; set; }

        public string Email
        {
            get => Data.Attribute("email").Value;
            set => Data.SetAttributeValue("email", value);
        }

        public WorkflowUser(XElement data)
            : base(data)
        {

        }

        public WorkflowUser()
            : base(new XElement(TagName))
        {

        }

        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            //Initializing the States list
            Roles = new XmlModelList<RoleReference>(GetElement("roles", true));
        }

        public RoleReference AddRoleReference(Guid roleId, string exceptionMessage)
        {
            if (Roles.Where(r => r.RefId == roleId).Any())
                throw new Exception(string.Format("Reference to role {0}: {1} already exists.", roleId, exceptionMessage));

            RoleReference newRef = new RoleReference() { RefId = roleId };
            Roles.Add(newRef);
            return newRef;
        }


    }
}
