using Catfish.Core.Models.Contents.Data;
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

        public enum eAccess { Restricted = 0, AnyLoggedIn, Public }

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


        public XmlModelList<RoleReference> Authorizations { get; set; }
        public XmlModelList<PostAction> PostActions { get; set; }
        public XmlModelList<Param> Params { get; set; }
        public XmlModelList<StateRef> States { get; set; }

        public GetAction(XElement data)
            : base(data)
        {

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
            States = new XmlModelList<StateRef>(stateListDefinition, true, "state-refs");

            //Initializing the authorizations list
            XElement authorizationsListDefinition = GetElement("authorizations", true);
            Authorizations = new XmlModelList<RoleReference>(authorizationsListDefinition, true, "role-refs");

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
        public RoleReference AddAuthorization(Guid refId)
        {
            if (Authorizations.Where(st => st.Id == refId).Any())
                throw new Exception(string.Format("Authorization already exists."));

            RoleReference newAuthorization = new RoleReference() { RefId = refId };
            Authorizations.Add(newAuthorization);
            return newAuthorization;
        }

        public StateRef AddStateReferances(Guid refId)
        {
            if (States.Where(st => st.Id == refId).Any())
                throw new Exception(string.Format("State already exists."));

            StateRef newState = new StateRef() { RefId = refId };
            States.Add(newState);
            return newState;
        }

    }
}
