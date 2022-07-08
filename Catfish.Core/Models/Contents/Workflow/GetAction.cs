using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Fields;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class GetAction : XmlModel
    {
        public static readonly string TagName = "get-action";
        public static readonly string LableAtt = "link-lable";
        public static readonly string FunctionAtt = "function";
        public static readonly string GroupAtt = "group";
        public static readonly string AccessAtt = "access";

        public enum eAccess { Restricted = 0, AnyFromDomain, AnyLoggedIn, Public }

        public string LinkLabel
        {
            get => GetAttribute(LableAtt, null as string);
            set => SetAttribute(LableAtt, value);
        }

        public string Function
        {
            get => GetAttribute(FunctionAtt, null as string);
            set => SetAttribute(FunctionAtt, value);
        }
        public string Group
        {
            get => GetAttribute(GroupAtt, null as string);
            set => SetAttribute(GroupAtt, value);
        }

        public eAccess Access
        {
            get => GetAttribute<eAccess>(AccessAtt, eAccess.Restricted);
            set => SetAttribute(AccessAtt, value.ToString());
        }

        public XmlModelList<PostAction> PostActions { get; set; }
        public XmlModelList<Param> Params { get; set; }
        public XmlModelList<StateRef> States { get; set; }

        public GetAction(XElement data)
            : base(data)
        {
            Initialize(eGuidOption.Ignore);
        }
        public GetAction()
            : base(new XElement(TagName))
        {

        }

        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            //Initializing the params list
            XElement paramsListDefinition = GetElement("params", true);
            Params = new XmlModelList<Param>(paramsListDefinition, true, "param");

            //Initializing the postaction list
            XElement postactionsListDefinition = GetElement("post-actions", true);
            PostActions = new XmlModelList<PostAction>(postactionsListDefinition, true, "post-action");

            //Initializing the state list
            XElement stateListDefinition = GetElement("states", true);
            States = new XmlModelList<StateRef>(stateListDefinition, true, "state-ref");           
        }

        public Param AddTemplate(Guid dataItemFormTemplateId, string exceptionMessage)
        {
            if (Params.Where(p => p.TemplateId == dataItemFormTemplateId).Any())
                throw new Exception(string.Format("Form Template {0} already exists.", exceptionMessage));

            Param newTemplate = new Param() { TemplateId = dataItemFormTemplateId };
            Params.Add(newTemplate);
            return newTemplate;
        }

        public PostAction AddPostAction(string buttonLable, string function)
        {
            if (PostActions.FindByAttribute(PostAction.LableAtt, buttonLable) != null)
                    throw new Exception(string.Format("Post action {0} already exists.", buttonLable));

            PostAction newPostAction = new PostAction() { ButtonLabel = buttonLable, Function = function };
            PostActions.Add(newPostAction);
            return newPostAction;
        }

        public PostAction AddPostAction(string buttonLable, string function, string successMessage)
        {
            if (PostActions.FindByAttribute(PostAction.LableAtt, buttonLable) != null)
                throw new Exception(string.Format("Post action {0} already exists.", buttonLable));

            PostAction newPostAction = new PostAction() { ButtonLabel = buttonLable, Function = function, SuccessMessage = successMessage};
            PostActions.Add(newPostAction);
            return newPostAction;
        }

        public StateRef GetStateReference(Guid stateId, bool createIfNotExist)
        {
            StateRef stateRef = States.Where(sr => sr.RefId == stateId).FirstOrDefault();
            if (stateRef == null && createIfNotExist)
                stateRef = AddStateReferances(stateId);
            return stateRef;
        }

        public RoleReference AddAuthorizedRole(Guid stateId, Guid roleId)
        {
            StateRef stateRef = GetStateReference(stateId, true);

            if (stateRef.AuthorizedRoles.Where(roleRef => roleRef.RefId == roleId).Any())
                throw new Exception(string.Format("Authorization already exists."));

            RoleReference newAuthorization = new RoleReference() { RefId = roleId };
            stateRef.AuthorizedRoles.Add(newAuthorization);
            return newAuthorization;
        }

        public EmailDomain AddAuthorizedDomain(Guid stateId, string domain)
        {
            StateRef stateRef = States.Where(sr => sr.RefId == stateId).FirstOrDefault();
            if (stateRef == null)
                stateRef = AddStateReferances(stateId);

            EmailDomain d = new EmailDomain() { Value = domain };
            stateRef.AuthorizedDomains.Add(d);
            return d;
        }

        public StateRef AddStateReferances(Guid refId)
        {
            if (States.Where(st => st.Id == refId).Any())
                throw new Exception(string.Format("State already exists."));

            StateRef newState = new StateRef() { RefId = refId };
            States.Add(newState);
            return newState;
        }

        public bool IsAuthorizedByDomain(Guid stateId, string userEmail)
        {
            StateRef stateRef = States.Where(st => st.RefId == stateId).FirstOrDefault();

            if (stateRef == null)
                throw new Exception(string.Format("Requested state does not exist within the GetAction."));

            if (!string.IsNullOrWhiteSpace(userEmail) && stateRef.AuthorizedDomains.Count > 0)
            {
                string emailDomain = userEmail.Substring(userEmail.IndexOf("@"));                 
                return stateRef.AuthorizedDomains.Where(d => d.Value == emailDomain).Any(); ;
            }
            return false;
        }

        public bool IsAuthorizedByEmailField(Entity entity, string userEmail)
        {
            var stateId = entity.StatusId;
            StateRef stateRef = States.FirstOrDefault(st => st.RefId == stateId);

            if (stateRef == null)
                return false;

            foreach(var fieldRef in stateRef.AuthorizedEmailFields)
            {
                var dataItem = entity.DataContainer.FirstOrDefault(di => di.TemplateId == fieldRef.FieldContainerId);
                if (dataItem != null)
                {
                    var emailFieldVals = dataItem.Fields
                        .Where(df => df.Id == fieldRef.FieldId)
                        .Select(df => df as EmailField)
                        .SelectMany(ef => ef.Values)
                        .Select(txt => txt.Value)
                        .ToList();

                    if (emailFieldVals.Contains(userEmail))
                        return true;
                }
            }

            return false;
        }

    }
}
