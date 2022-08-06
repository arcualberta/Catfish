import { defineStore } from 'pinia';

import { Guid } from "guid-typescript";

import { Workflow } from '../models/'

export const useWorkflowBuilderStore = defineStore('WorkflowBuilderStore', {
    state: () => ({
        workflow: null as Workflow | null,
        transientMessage: null as string | null,
        transientMessageClass: null as string | null
    }),
    actions: {
        loadWorkflow(id: Guid) {
            const api = `https://localhost:5020/api/workflow/${id}`;
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
            let api = "https://localhost:5020/api/workflow";
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
    }
});