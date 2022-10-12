import { Guid } from 'guid-typescript';
import { defineStore } from 'pinia';
import { Entity, TemplateEntry } from '../models';
import { EntityTemplate } from '../../entity-template-builder/models'
import { default as config } from "@/appsettings";
import { eEntityType } from '@/components/shared/constants';
import { createFormData } from '@/components/shared/form-helpers'
import { Form, FormData as FormDataModel } from '@/components/shared/form-models'
import { TransientMessageModel } from '../../shared/components/transient-message/models'
import router from '@/router';

import { useFormSubmissionStore } from '@/components/form-submission/store';
export const useEntityEditorStore = defineStore('EntityEditorStore', {
    state: () => ({
        id: null as Guid | null,
        templates: [] as TemplateEntry[],
        entityTemplate: null as EntityTemplate | null,
        entity: null as Entity | null,
        transientMessageModel: {} as TransientMessageModel,
       
    }),
    actions: {
        loadTemplates() {
            const api = `${config.dataRepositoryApiRoot}/api/entity-templates/`;

            fetch(api, {
                method: 'GET'
            })
                .then(response => response.json())
                .then(data => {
                    this.templates = data as TemplateEntry[];
                })
                .catch((error) => {
                    console.error('Load Templates API Error:', error);
                });
        },
        createNewEntity() {
            this.entity = {
                id: Guid.createEmpty().toString() as unknown as Guid,
                templateId: Guid.createEmpty().toString() as unknown as Guid,
                entityType: eEntityType.Unknown,
                data: [] as FormDataModel[],
                files: [] as File[]
            }
        },
        loadTemplate(templateId: Guid) {
            const api = `${config.dataRepositoryApiRoot}/api/entity-templates/${templateId}`;

            fetch(api, {
                method: 'GET'
            })
                .then(response => response.json())
                .then(data => {
                    this.entityTemplate = data as EntityTemplate;
                })
                .catch((error) => {
                    console.error('Load Template API Error:', error);
                });
        },
        saveEntity(){
            //console.log("save form template: ", JSON.stringify(this.template));
            const newEntity = this.entity?.id?.toString() === Guid.EMPTY;
           
            let api = config.dataRepositoryApiRoot + "/api/entities";
            let method = "";
            if (newEntity) {
                console.log("Saving new entity.");
                //console.log(JSON.stringify(this.entity));
                if(this.entity?.id?.toString() === Guid.EMPTY){
                    this.entity.id = Guid.create().toString() as unknown as Guid;
                }
                method = "POST";
            }
            else {
                console.log("Updating existing entity.")
                api = `${api}/${this.entity?.id}`
                method = "PUT";
            }
            //get files if any
            const formSubmissionstore = useFormSubmissionStore();           
            let attachedFiles = formSubmissionstore.files as File[];

            var formData = new FormData();
            formData.append('value', JSON.stringify(this.entity))
            attachedFiles?.forEach(file => {
                formData.append('files', file);
            })

             //  this.entity!.files = attachedFiles.slice();
            // attachedFiles.forEach((file)=>{
            //        this.entity?.files?.push(file);
            // });
           //formData.append('value', JSON.stringify(this.entity));

            fetch(api, {
                body: formData, //JSON.stringify(this.entity),
                method: method,
                headers: {
                        'encType': 'multipart/form-data'
                },
            })
            .then(response => {
                if (response.ok) {
                    
                    this.transientMessageModel.message = "The entity saved successfully"
                    this.transientMessageModel.messageClass = "success"
                    router.push(`/edit-entity-editor/${this.entity!.id}`)
                }
                else {
                    if (newEntity && this.entity)
                        this.entity.id = Guid.EMPTY as unknown as Guid;

                    this.transientMessageModel.messageClass = "danger"
                    switch (response.status) {
                        case 400:
                            this.transientMessageModel.message = "Bad request. Failed to save the entity";
                            break;
                        case 404:
                            this.transientMessageModel.message = "Entity not found";
                            break;
                        case 500:
                            this.transientMessageModel.message = "An internal server error occurred. Failed to save the entity"
                            break;
                        default:
                            this.transientMessageModel.message = "Unknown error occured. Failed to save the entity"
                            break;
                    }
                }
            })
            .catch((error) => {
                if (newEntity && this.entity)
                        this.entity.id = Guid.EMPTY as unknown as Guid;
                this.transientMessageModel.message = "Unknown error occurred"
                this.transientMessageModel.messageClass = "danger"
                console.error('Save/Update Entity API Error:', error);
            });
        },
        loadEntity(entityId: Guid) {
            const api = `${config.dataRepositoryApiRoot}/api/entities/${entityId}`;

            fetch(api, {
                method: 'GET'
            })
                .then(response => response.json())
                .then(data => {
                    this.entity = data as Entity;
                })
                .catch((error) => {
                    console.error('Load Entity API Error:', error);
                });
        },
    },
    getters: {
        titleField: (state) => {
            const fieldEntry = state?.entityTemplate?.entityTemplateSettings?.titleField;
            const field = state.entityTemplate?.forms?.filter(form => form.id === fieldEntry?.formId)[0]
                ?.fields.filter(field => field.id == fieldEntry?.fieldId)[0];
            return field;
        },
        descriptionField: (state) => {
            const fieldEntry = state?.entityTemplate?.entityTemplateSettings?.descriptionField;
            const field = state.entityTemplate?.forms?.filter(form => form.id === fieldEntry?.formId)[0]
                ?.fields.filter(field => field.id == fieldEntry?.fieldId)[0];
            return field;
        },
        mediaField: (state) => {
            const fieldEntry = state?.entityTemplate?.entityTemplateSettings?.mediaField;
            const field = state.entityTemplate?.forms?.filter(form => form.id === fieldEntry?.formId)[0]
                ?.fields.filter(field => field.id == fieldEntry?.fieldId)[0];
            return field;
        },
        getFiles: (state)=>{
              const formSubmissionstore = useFormSubmissionStore();
              return formSubmissionstore.files;
        }
    }
});