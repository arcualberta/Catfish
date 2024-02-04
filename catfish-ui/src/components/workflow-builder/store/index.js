import { defineStore } from 'pinia';
import { Guid } from "guid-typescript";
import { default as config } from "@/appsettings";
import { WebClient } from '@/api/webClient';
import { EntityTemplateProxy } from "@/api/entityTemplateProxy";
import { WorkflowProxy } from '@/api/workflowProxy';
export const useWorkflowBuilderStore = defineStore('WorkflowBuilderStore', {
    state: () => ({
        workflow: null,
        transientMessage: null,
        transientMessageClass: null,
        entityTemplates: [],
        users: [],
        showActionPanel: false,
        showTriggerPanel: false,
        showPopupPanel: false,
        entityTemplate: null,
    }),
    actions: {
        createNewWorkflow() {
            let newState = {
                id: Guid.create().toString(),
                name: "Empty State",
                description: "This is initial state"
            };
            this.workflow = {
                id: Guid.EMPTY,
                name: "",
                description: "",
                states: [],
                roles: [],
                emailTemplates: [],
                actions: [],
                triggers: [],
                entityTemplateId: Guid.EMPTY,
                popups: []
            };
            this.workflow.states.push(newState);
        },
        async loadTemplate(templateId) {
            console.log("templateId", templateId);
            if (templateId === Guid.EMPTY)
                return;
            this.entityTemplate = await EntityTemplateProxy.Get(templateId);
        },
        async loadWorkflow(id) {
            this.workflow = await WorkflowProxy.Get(id);
        },
        async saveWorkflow() {
            if (!this.workflow) {
                console.error("Cannot save null workflow.");
                return;
            }
            const newWorkflow = this.workflow?.id === Guid.EMPTY;
            var response;
            if (newWorkflow) {
                response = await WorkflowProxy.Post(this.workflow);
            }
            /* console.log(this.workflow?.id)
             let api = `${config.dataRepositoryApiRoot}/api/workflow`;
             console.log(api)
             let promise = newWorkflow ? WebClient.postJson(api, this.workflow) : WebClient.putJson(`${api}/${this.workflow.id}`, this.workflow)
            */
            // promise.then(response => {
            if (response) {
                this.transientMessage = "The form saved successfully";
                this.transientMessageClass = "success";
            }
            else {
                this.transientMessageClass = "danger";
                this.transientMessage = "The form saved fail";
                /*switch (response.status) {
                    case 400:
                        this.transientMessage = "Bad request. Failed to save the workflow";
                        break;
                    case 404:
                        this.transientMessage = "Workflow not found";
                        break;
                    case 500:
                        this.transientMessage = "An internal server error occurred. Failed to save the workflow"
                        break;
                    default:
                        this.transientMessage = "Unknown error occured. Failed to save the workflow"
                        break;
                }*/
            }
            /*  })
              .catch((error) => {
                  this.transientMessage = "Unknown error occurred"
                  this.transientMessageClass = "danger"
                  console.error('Workflow Save API Error:', error)
              });*/
        },
        async loadEntityTemplates() {
            this.entityTemplates = await EntityTemplateProxy.List();
        },
        loadUsers() {
            const api = `${config.authorizationApiRoot}/api/Users`;
            WebClient.get(api)
                .then(response => response.json())
                .then(data => {
                this.users = data;
            })
                .catch((error) => {
                console.error('Load users API Error:', error);
            });
        },
    }
});
//# sourceMappingURL=index.js.map