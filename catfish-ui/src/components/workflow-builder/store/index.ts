import { defineStore } from 'pinia';
import { Guid } from "guid-typescript";
import { default as config } from "@/appsettings";
import { EmailTemplate, Workflow, WorkflowState, WorkflowRoles, WorkflowAction, WorkflowTrigger} from '../models/'
import { TemplateEntry } from '@/components/entity-editor/models';

export const useWorkflowBuilderStore = defineStore('WorkflowBuilderStore', {
    state: () => ({
        workflow: null as Workflow | null,
        transientMessage: null as string | null,
        transientMessageClass: null as string | null,
        entityTemplates: [] as TemplateEntry[],

        //states: [] as WorkflowState[] | null,
        //roles: [] as WorkflowRoles[] | null,
        emailTemplates: [] as EmailTemplate[] | null
    }),
    actions: {
        createNewWorkflow(){
            this.workflow = {
                id: Guid.EMPTY as unknown as Guid,
                name: "New Workflow Template",
                description: "Description about this new Wotkflow Template",
                states: [] as WorkflowState[],
                roles:[] as WorkflowRoles[],
                emailTemplates:[] as EmailTemplate[],
                actions:[] as WorkflowAction[],
                triggers:[] as WorkflowTrigger[],
                entityTemplateId:Guid.EMPTY as unknown as Guid,
                popups: Object 
            }
        },
        loadWorkflow(id: Guid) {
            const api = `${config.dataRepositoryApiRoot}/api/workflow/${id}`;//`https://localhost:5020/api/workflow/${id}`;
            fetch(api, {
                method: 'GET'
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

            const newWorkflow = this.workflow?.id?.toString() === Guid.EMPTY;
            let api = `${config.dataRepositoryApiRoot}/api/workflow`;
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

            fetch(api,
                {
                    body: JSON.stringify(this.workflow),
                    method: method,
                    headers: {
                        'encType': 'multipart/form-data',
                        'Content-Type': 'application/json'
                    },
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
                method: 'GET'
            })
            .then(response => response.json())
            .then(data => {
                this.entityTemplates = data;
            })
            .catch((error) => {
                console.error('Load Entity Templates API Error:', error);
            });

        },
    }
});