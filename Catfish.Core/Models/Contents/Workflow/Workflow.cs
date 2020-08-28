﻿using System;
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
        public XmlModelList<WorkflowRole> Roles { get; set; }
        public XmlModelList<WorkflowUser> Users { get; set; }
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

            Roles = new XmlModelList<WorkflowRole>(GetElement("roles", true));

            Users = new XmlModelList<WorkflowUser>(GetElement("users", true));

            //Initializing the Triggers list
            XElement triggerListDefinition = GetElement("triggers", true);
            Triggers = new XmlModelList<Trigger>(triggerListDefinition, true, "trigger");

            //Initializing the actions list
            XElement actionListDefinition = GetElement("actions", true);
            Actions = new XmlModelList<GetAction>(actionListDefinition, true, "action");

        }

        public State AddState(string stateValue)
        {
            if (States.Where(st => st.Value == stateValue).Any())
                throw new Exception(string.Format("State {0} already exists.", stateValue));

            State newState = new State() { Value = stateValue };
            States.Add(newState);
            return newState;
        }

        public WorkflowRole AddRole(string roleValue)
        {
            if (Roles.Where(x => x.Value == roleValue).Any())
                throw new Exception(string.Format("Role {0} already exists.", roleValue));

            WorkflowRole newRole = new WorkflowRole() { Value = roleValue };
            Roles.Add(newRole);
            return newRole;
        }

        public WorkflowUser AddUser(string userEmail)
        {
            if (Users.Where(x => x.Email == userEmail).Any())
                throw new Exception(string.Format("User {0} already exists.", userEmail));

            WorkflowUser newUser = new WorkflowUser(){ Email = userEmail };
            Users.Add(newUser);
            return newUser;
        }

        public EmailTrigger AddTrigger(string name, string function)
        {
            if (Triggers.Where(x => x.Name== name).Any())
                throw new Exception(string.Format("Trigger {0} already exists.", name));

            EmailTrigger newTrigger = new EmailTrigger() { Name = name, Function=function };
            Triggers.Add(newTrigger);
            return newTrigger;
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
    }
}
