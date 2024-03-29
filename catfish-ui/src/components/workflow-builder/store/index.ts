import { defineStore } from 'pinia';
import { Guid } from "guid-typescript";
import { EntityTemplate } from '../../entity-template-builder/models'
import { default as config } from "@/appsettings";
import { TemplateEntry } from '@/components/entity-editor/models';
import { Workflow, WorkflowState, WorkflowRole, WorkflowEmailTemplate, WorkflowTrigger, WorkflowAction, WorkflowPopup, UserInfo } from '../models/'
import { useLoginStore } from '@/components/login/store';
import { WebClient } from '@/api/webClient';
import {EntityTemplateProxy} from "@/api/entityTemplateProxy";
import { WorkflowProxy } from '@/api/workflowProxy';

export const useWorkflowBuilderStore = defineStore('WorkflowBuilderStore', {
    state: () => ({
        workflow : null as Workflow | null,
        transientMessage : null as string | null,
        transientMessageClass : null as string | null,
        entityTemplates : [] as TemplateEntry[],
        users : [] as UserInfo[],
        showActionPanel : false as boolean,
        showTriggerPanel : false as boolean,
        showPopupPanel : false as boolean,
        entityTemplate: null as EntityTemplate | null,
    }),
    actions: {
        createNewWorkflow() {
            let newState= {
                id : Guid.create().toString() as unknown as Guid,
                name : "Empty State",
                description : "This is initial state"
            } as WorkflowState;
            this.workflow = {
                id : Guid.EMPTY as unknown as Guid,
                name : "",
                description : "",
                states : [] as WorkflowState[],
                roles : [] as WorkflowRole[],
                emailTemplates : [] as WorkflowEmailTemplate[],
                actions : [] as WorkflowAction[],
                triggers : [] as WorkflowTrigger[],
                entityTemplateId : Guid.EMPTY as unknown as Guid,
                popups : [] as WorkflowPopup[]
            }
            this.workflow.states.push(newState);
        },
        async loadTemplate(templateId: Guid) {
            console.log("templateId",templateId)
            if(templateId === Guid.EMPTY as unknown as Guid)
                return;

                this.entityTemplate = await EntityTemplateProxy.Get(templateId);
        },
        async loadWorkflow(id: Guid) {

               this.workflow = await  WorkflowProxy.Get(id);

        },
        async saveWorkflow() {
            if (!this.workflow) {
                console.error("Cannot save null workflow.")
                return;
            }
            
            const newWorkflow = this.workflow?.id === Guid.EMPTY as unknown as Guid;
            var response;
            if(newWorkflow){
                response = await WorkflowProxy.Post<Workflow>(this.workflow as Workflow)
            }
           /* console.log(this.workflow?.id)
            let api = `${config.dataRepositoryApiRoot}/api/workflow`;
            console.log(api)
            let promise = newWorkflow ? WebClient.postJson(api, this.workflow) : WebClient.putJson(`${api}/${this.workflow.id}`, this.workflow)
           */
           // promise.then(response => {

                    if (response) {
                        this.transientMessage = "The form saved successfully"
                        this.transientMessageClass = "success"
                    }
                    else {
                        this.transientMessageClass = "danger"
                        this.transientMessage = "The form saved fail"
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