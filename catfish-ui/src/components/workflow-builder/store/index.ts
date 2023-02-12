import { defineStore } from 'pinia';
import { Guid } from "guid-typescript";
import { EntityTemplate } from '../../entity-template-builder/models'
import { default as config } from "@/appsettings";
import { TemplateEntry } from '@/components/entity-editor/models';
import { Workflow, WorkflowState, WorkflowRole, WorkflowEmailTemplate, WorkflowTrigger, WorkflowAction, WorkflowPopup } from '../models/'

export const useWorkflowBuilderStore = defineStore('WorkflowBuilderStore', {
    state: () => ({
        workflow : null as Workflow | null,
        transientMessage : null as string | null,
        transientMessageClass : null as string | null,
        entityTemplates : [] as TemplateEntry[],
        showActionPanel : false as boolean,
        showTriggerPanel : false as boolean,
        showPopupPanel : false as boolean,
        entityTemplate: null as EntityTemplate | null
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
        loadTemplate(templateId: Guid) {
            console.log("templateId",templateId)
            if(templateId === Guid.EMPTY as unknown as Guid)
                return;

            let webRoot = config.dataRepositoryApiRoot;
            const api = `${webRoot}/api/entity-templates/${templateId}`;
            console.log(api)

            fetch(api, {
                method: 'GET',
                headers: {
                    'Authorization': `bearer ${this.jwtToken}`
                }
            })
                .then(response => response.json())
                .then(data => {
                    this.entityTemplate = data as EntityTemplate;
                    console.log("entityTemplate", this.entityTemplate)
                })
                .catch((error) => {
                    console.error('Load Template API Error:', error);
                });
        },
        loadWorkflow(id: Guid) {
            const api = `${config.dataRepositoryApiRoot}/api/workflow/${id}`;//`https://localhost:5020/api/workflow/${id}`;
            fetch(api, {
                method: 'GET',
                headers: {
                    'Authorization': `bearer ${this.jwtToken}`
                }
            })
                .then(response => response.json())
                .then(data => {
                    this.workflow = data;
                })
                .catch((error) => {
                    console.error('Load Workflow API Error:', error);
                });

        },
        saveWorkflow() {
            if (!this.workflow) {
                console.error("Cannot save null workflow.")
                return;
            }
            
            const newWorkflow = this.workflow?.id === Guid.EMPTY as unknown as Guid;
            console.log(this.workflow?.id)
            let api = `${config.dataRepositoryApiRoot}/api/workflow`;
            console.log(api)
            //let api = "https://localhost:5020/api/workflow";
            let method = "";
            if (newWorkflow) {
                console.log("Saving new workflow.")
                method = "POST";
            }
            else {
                console.log("Updating existing workflow.")
                api = `${api}/${this.workflow.id}`
                method = "PUT";
            }
            console.log(JSON.stringify(this.workflow))
            fetch(api,
                {
                    body: JSON.stringify(this.workflow),
                    method: method,
                    headers: {
                        'encType': 'multipart/form-data',
                        'Content-Type': 'application/json',
                        'Authorization': `bearer ${this.jwtToken}`
                    }
                })
                .then(response => {
                    if (response.ok) {
                        this.transientMessage = "The form saved successfully"
                        this.transientMessageClass = "success"
                    }
                    else {
                        this.transientMessageClass = "danger"
                        switch (response.status) {
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
                        }
                    }
                })
                .catch((error) => {
                    this.transientMessage = "Unknown error occurred"
                    this.transientMessageClass = "danger"
                    console.error('Workflow Save API Error:', error)
                });
        },
        loadEntityTemplates() {
            const api = `${config.dataRepositoryApiRoot}/api/entity-templates`;//`https://localhost:5020/api/workflow/${id}`;
            fetch(api, {
                method: 'GET',
                headers: {
                    'Authorization': `bearer ${this.jwtToken}`
                }
            })
                .then(response => response.json())
                .then(data => {
                    this.entityTemplates = data;
                })
                .catch((error) => {
                    console.error('Load Entity Templates API Error:', error);
                });

        },
    },
    getters:{
        jwtToken(state){
            return localStorage.getItem("catfishJwtToken");
        }
    }
});