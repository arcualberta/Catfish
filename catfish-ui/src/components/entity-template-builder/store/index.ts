import { Guid } from 'guid-typescript';
import { defineStore } from 'pinia';
import { EntityTemplate } from '../models';
import { eState } from "../../shared/constants";
import { default as config } from "@/appsettings";
import router from '@/router';
import { FieldEntry, FormTemplate } from '../../shared/form-models';
import { FormEntry } from '../../shared';
import { TransientMessageModel } from '../../shared/components/transient-message/models'

export const useEntityTemplateBuilderStore = defineStore('EntityTemplateBuilderStore', {
    state: () => ({
        id: null as Guid | null,
        template: null as EntityTemplate | null,
        formEntries: [] as FormEntry[],
        transientMessageModel: {} as TransientMessageModel,
        forms: [] as FormTemplate[],
        apiRoot: null as string |null
    }),
    actions: {
        newTemplate() {
            this.template = {
                id: Guid.EMPTY as unknown as Guid,
                name: "New Entity Template",
                description: "Description about this new Entity Template",
                created: new Date(),
                updated: new Date(),
                state: eState.Unpublished,
                forms:[],
                entityTemplateSettings: {
                    metadataForms: [],
                    dataForms: [],
                    titleField: {} as FieldEntry,
                    descriptionField: {} as FieldEntry,
                    mediaField: {} as FieldEntry
                }
            };
        },
        associateForm(formId: Guid) {
            if (this.forms.findIndex(form => form.id === formId) < 0) {
                //this.getApiRoot => https://localhost:40520/api/entityTemlate
            let webRoot = this.getApiRoot.split("/")[0]+"//" + this.getApiRoot.split("/")[2];
                const api = `${webRoot}/api/forms/${formId}`;
               // console.log("loading form: ", api);

                fetch(api, {
                    method: 'GET'
                })
                    .then(response => response.json())
                    .then(data => {
                        this.forms.push(data as FormTemplate)
                    })
                    .catch((error) => {
                        console.error('Load Form API Error:', error);
                    });
            }
        },
        loadFormEntries() {
            //this.getApiRoot => localhost:40520/api/entity-temlates
            let webRoot = this.getApiRoot.split("/")[0]+"//" + this.getApiRoot.split("/")[2];
            const api = `${webRoot}/api/forms`;
            console.log("loading forms: ", api);

            fetch(api, {
                method: 'GET'
            })
                .then(response => response.json())
                .then(data => {
                    this.formEntries = data as FormEntry[]
                })
                .catch((error) => {
                    console.error('Load Forms API Error:', error);
                });
        },
        loadTemplate(id: Guid) {
            const api = `${this.getApiRoot}/${id}`;
            console.log("loading entityTemplate: ", api);

            fetch(api, {
                method: 'GET'
            })
                .then(response => response.json())
                .then(data => {
                    //console.log(data);
                    this.template = data as EntityTemplate;
                })
                .catch((error) => {
                    console.error('Load Entity Template API Error:', error);
                });
        },
        saveTemplate(){
            //console.log("save form template: ", JSON.stringify(this.template));
            const newTemplate = this.template?.id?.toString() === Guid.EMPTY;
           
            let api = this.getApiRoot;
            let method = "";
            if (newTemplate) {
                console.log("Saving new template.");
                if(this.template?.id?.toString() === Guid.EMPTY){
                    this.template.id = Guid.create().toString() as unknown as Guid;
                }
                method = "POST";
            }
            else {
                console.log("Updating existing template.")
                api = `${api}/${this.template?.id}`
                method = "PUT";
            }
            fetch(api, {
                body: JSON.stringify(this.template),
                method: method,
                headers: {
                        'encType': 'multipart/form-data',
                        'Content-Type': 'application/json'
                },
            })
            .then(response => {
                if (response.ok) {
                    //router.push(`/edit-entity-template/${this.template!.id}`)
                    this.transientMessageModel.message = "The template saved successfully"
                    this.transientMessageModel.messageClass = "success"
                }
                else {
                    if (newTemplate && this.template)
                        this.template.id = Guid.EMPTY as unknown as Guid;

                    this.transientMessageModel.messageClass = "danger"
                    switch (response.status) {
                        case 400:
                            this.transientMessageModel.message = "Bad request. Failed to save the form";
                            break;
                        case 404:
                            this.transientMessageModel.message = "Form not found";
                            break;
                        case 500:
                            this.transientMessageModel.message = "An internal server error occurred. Failed to save the form"
                            break;
                        default:
                            this.transientMessageModel.message = "Unknown error occured. Failed to save the form"
                            break;
                    }
                }
            })
            .catch((error) => {
                if (newTemplate && this.template)
                        this.template.id = Guid.EMPTY as unknown as Guid;
                this.transientMessageModel.message = "Unknown error occurred"
                this.transientMessageModel.messageClass = "danger"
                console.error('Save/Update Entity Template API Error:', error);
            });
        },
        deleteFormEntry(id: Guid) {
            let index = this.template!.entityTemplateSettings.dataForms!.findIndex(fe => fe.id === id);
            if (index >= 0)
                this.template!.entityTemplateSettings.dataForms!.splice(index, 1);
            else {
                index = this.template!.entityTemplateSettings.metadataForms!.findIndex(fe => fe.id === id);
                if (index >= 0)
                    this.template!.entityTemplateSettings.metadataForms!.splice(index, 1);
            }
        },
        setApiRoot(api: string){
            this.apiRoot = api;
        }
    },
    getters:{
        getApiRoot(state){
            return state.apiRoot? state.apiRoot : config.dataRepositoryApiRoot + "/api/entity-templates";
        }
    }    
});