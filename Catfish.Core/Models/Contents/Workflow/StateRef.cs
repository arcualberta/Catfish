using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class StateRef : WorkflowReferrence
    {
        public static readonly string TagName = "state-ref";

        public XmlModelList<RoleReference> AuthorizedRoles { get; set; }
        public XmlModelList<EmailDomain> AuthorizedDomains { get; set; }
        public XmlModelList<FieldReference> AuthorizedEmailFields { get; set; }

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
            AuthorizedEmailFields = new XmlModelList<FieldReference>(authorizationsListDefinition, true, FieldReference.TagName);
        }

        #region Role-based authorization
        public StateRef AddAuthorizedRole(Guid roleId)
        {
            if (AuthorizedRoles.Where(roleRef => roleRef.RefId == roleId).Any())
                throw new Exception(string.Format("Authorization is already granted."));

            RoleReference newAuthorization = new RoleReference() { RefId = roleId };
            AuthorizedRoles.Add(newAuthorization);

            return this;
        }

        public StateRef RemoveAuthorizedRole(Guid roleId)
        {
            var roles = AuthorizedRoles.Where(roleRef => roleRef.RefId == roleId).ToList();
            foreach(var role in roles)
                AuthorizedRoles.Remove(role);

            return this;
        }

        public bool IsAuthorizedByRole(Guid roleId)
        {
            return AuthorizedRoles.Where(roleRef => roleRef.RefId == roleId).Any();
        }
        #endregion

        #region Domain-based authorization
        public StateRef AddAuthorizedDomain(string domain)
        {
            if (AuthorizedDomains.Where(d => d.Value == domain).Any())
                throw new Exception(string.Format("Authorization is already granted."));

            EmailDomain d = new EmailDomain() { Value = domain };
            AuthorizedDomains.Add(d);

            return this;
        }

        public StateRef RemoveAuthorizedDomain(string domain)
        {
            var domains = AuthorizedDomains.Where(d => d.Value == domain).ToList();
            foreach (var d in domains)
                AuthorizedDomains.Remove(d);

            return this;
        }

        public StateRef AddAuthorizedUserByEmailField(Guid dataItemId, Guid emailFieldId)
        {
            if (AuthorizedEmailFields.Where(fr => fr.DataItemId == dataItemId && fr.FieldId == emailFieldId).Any())
                throw new Exception(string.Format("Authorization is already granted."));

            FieldReference fr = new FieldReference() { DataItemId = dataItemId, FieldId = emailFieldId };
            AuthorizedEmailFields.Add(fr);

            return this;
        }


        public bool IsAuthorizedByDomain(string domain)
        {
            return AuthorizedDomains.Where(d => d.Value == domain).Any();
        }
        #endregion

        #region Authorization by ownerhip
        protected RoleReference GetOwnerRole()
        {
            return AuthorizedRoles
                .Where(roleRef => roleRef.Data.Attribute(RoleReference.OwnerAttribute) != null)
                .FirstOrDefault();
        }
        public StateRef AddOwnerAuthorization()
        {
            RoleReference ownerRoleRef = GetOwnerRole();

            if (ownerRoleRef == null)
                AuthorizedRoles.Add(new RoleReference() { Owner = true, RefId = Guid.Empty });
            else
                ownerRoleRef.Owner = true;

            return this;
        }

        public StateRef RemoveOwnerAuthorization()
        {
            RoleReference ownerRoleRef = GetOwnerRole();

            if (ownerRoleRef != null)
                AuthorizedRoles.Remove(ownerRoleRef);

            return this;
        }

        public bool IsOwnerAuthorized()
        {
            RoleReference ownerRoleRef = GetOwnerRole();
            return ownerRoleRef != null ? ownerRoleRef.Owner : false;
        }
        #endregion

    }
}
