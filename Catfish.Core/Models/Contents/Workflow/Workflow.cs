using Piranha.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class Workflow : XmlModel
    {
        public static readonly string TagName = "workflow";

        public XmlModelList<State> States { get; set; }
        public XmlModelList<WorkflowGroup> Groups { get; set; }
        public XmlModelList<WorkflowRole> Roles { get; set; }
        //public XmlModelList<WorkflowUser> Users { get; set; }
        public XmlModelList<GetAction> Actions { get; set; }
        public XmlModelList<Trigger> Triggers { get; set; }

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

            Groups = new XmlModelList<WorkflowGroup>(GetElement("groups", true));

            Roles = new XmlModelList<WorkflowRole>(GetElement("roles", true));

            //Users = new XmlModelList<WorkflowUser>(GetElement("users", true));

            //Initializing the Triggers list
            XElement triggerListDefinition = GetElement("triggers", true);
            Triggers = new XmlModelList<Trigger>(triggerListDefinition, true, "trigger");

            //Initializing the actions list
            XElement actionListDefinition = GetElement("actions", true);
            Actions = new XmlModelList<GetAction>(actionListDefinition, true, "get-action");

        }

        public State GetState(Guid stateId)
        {
            return States.Where(st => st.Id == stateId).FirstOrDefault();
        }

        public State AddState(SystemStatus status)
        {
            if (States.Where(st => st.Value == status.Status).Any())
                throw new Exception(string.Format("State {0} already exists.",status.Status));

            State newState = new State() { Value = status.Status, Id = status.Id };
            States.Add(newState);
            return newState;
        }

        ////public WorkflowGroup GetGroup(string value)
        ////{
        ////    return Groups.Where(gr => gr.Value == value).FirstOrDefault();
        ////}

        ////public WorkflowGroup AddGroup(string value)
        ////{
        ////    if (Groups.Where(gr => gr.Value == value).Any())
        ////        throw new Exception(string.Format("Group {0} already exists.", value));

        ////    WorkflowGroup newGroup = new WorkflowGroup() { Value = value };
        ////    Groups.Add(newGroup);
        ////    return newGroup;
        ////}

        public WorkflowRole GetRole(string value)
        {
            return Roles.Where(r => r.Value == value).FirstOrDefault();
        }

        public WorkflowRole AddRole(Role role)
        {
            if (Roles.Where(r => r.Value == role.Name).Any())
                throw new Exception(string.Format("Role {0} already exists.", role.Name));

            WorkflowRole newRole = new WorkflowRole() { Value = role.Name, Id = role.Id };
            Roles.Add(newRole);
            return newRole;
        }


        //public WorkflowUser AddUser(string userEmail)
        //{
        //    if (Users.Where(x => x.Email == userEmail).Any())
        //        throw new Exception(string.Format("User {0} already exists.", userEmail));

        //    WorkflowUser newUser = new WorkflowUser(){ Email = userEmail };
        //    Users.Add(newUser);
        //    return newUser;
        //}

        //public T GetTrigger<T>(string value) where T : Trigger
        //{
        //    return Triggers.Where(tr => tr is T && tr.Name == value).FirstOrDefault();
        //}
        public EmailTrigger GetTrigger(string value)
        {
            return Triggers.Where(tr => typeof(EmailTrigger).IsAssignableFrom(tr.GetType())
                                     && tr.Name == value)
                            .Select(tr => tr as EmailTrigger)
                            .FirstOrDefault();
        }

        public EmailTrigger AddTrigger(string name, string function)
        {
            if (Triggers.Where(x => x.Name== name).Any())
                throw new Exception(string.Format("Trigger {0} already exists.", name));

            EmailTrigger newTrigger = new EmailTrigger() { Name = name, Function=function };
            Triggers.Add(newTrigger);
            return newTrigger;
        }

        public GetAction GetAction(string lable)
        {
            return Actions.Where(x => x.LinkLabel == lable).FirstOrDefault();
        }

        public GetAction AddAction(string lable, string function, string group)
        {
            if (Actions.Where(x => x.LinkLabel == lable).Any())
                throw new Exception(string.Format("Action {0} already exists.", lable));

            GetAction newAction = new GetAction() { LinkLabel = lable, Function = function , Group = group};
            Actions.Add(newAction);
            return newAction;
        }

        public List<string> GetWorkflowRoles()
        {
            List<string> roles = new List<string>();
            var roleList = Roles.ToList();

            foreach (var role in roleList)
                roles.Add(role.Value);
            return roles;
        }

        public List<string> GetWorkflowGroups()
        {
            List<string> groups = new List<string>();
            var groupList = Groups.ToList();

            foreach (var group in groupList)
                groups.Add(group.Value);
            return groups;
        }
        
    }

    
}
