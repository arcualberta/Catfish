import { Guid } from 'guid-typescript';
import { defineStore } from 'pinia';
import { EntityData, Relationship, TemplateEntry, EntitySearchResult } from '../models';
import { EntityTemplate } from '../../entity-template-builder/models'
import { default as config } from "@/appsettings";
import { eEntityType, eSearchTarget, eState } from '@/components/shared/constants';
import { createFormData } from '@/components/shared/form-helpers'
import { FormData as FormDataModel } from '@/components/shared/form-models'
import { TransientMessageModel } from '../../shared/components/transient-message/models'
import {FileReference} from '@/components/shared/form-models/field'
import router from '@/router';
import { getConcatenatedTitle, getConcatenatedDescription} from '@/components/shared/entity-helpers'

import { useFormSubmissionStore } from '@/components/form-submission/store';
import {useEntitySelectStore} from '../../shared/components/entity-selection-list/store'
import { useLoginStore } from '@/components/login/store';

export const useEntityEditorStore = defineStore('EntityEditorStore', {
    state: () => ({
        id: null as Guid | null,
        templates: [] as TemplateEntry[],
        entityTemplate: null as EntityTemplate | null,
        entity: null as EntityData | null,
        transientMessageModel: {} as TransientMessageModel,
        updatedFileKeys: [] as string[] | null,
        entitySearchResult: null as EntitySearchResult | null,
        storeId:(Guid.create()).toString(),
        apiRoot: null as string | null
    }),
    actions: {
        loadTemplates() {
            let webRoot = config.dataRepositoryApiRoot;
            const api = `${webRoot}/api/entity-templates/`;
            //const api = `${config.dataRepositoryApiRoot}/api/entity-templates/`;
            const loginStore = useLoginStore();
            const jwtToken=loginStore.jwtToken;
           // const header=new Headers();
           // header.append('Authorization', "bearer " + jwtToken as string)
            fetch(api, {
                method: 'GET',
                //mode: 'no-cors',
                headers: {
                    'Authorization': `bearer ${jwtToken}`,
                }
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
                subjectRelationships:[] as Relationship[],
                objectRelationships: [] as Relationship[],
                files: [] as File[],
                created: new Date(),
                updated: new Date(),
                title: "",
                description: "",
                state: eState.Active
              
            }
        },
        loadTemplate(templateId: Guid) {

            if(templateId.toString() === Guid.EMPTY)
                return;

            let webRoot = config.dataRepositoryApiRoot;
            const api = `${webRoot}/api/entity-templates/${templateId}`;
            console.log(api)

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
        addFileReference(file: File, fieldId: Guid){
            let fileKey="";
            this.entity?.data.forEach((frmd)=>{
                frmd.fieldData.forEach(fld => {
                    if(fld.fieldId.toString() === fieldId.toString()){
                        if(!fld.fileReferences)
                        fld.fileReferences= [] as FileReference[];
                        fileKey=this.entity!.id + "_" + fld.id + "_" + fld.fieldId;
                        this.updatedFileKeys?.push(fileKey);
                        let fileName=fileKey+ "_" + file.name;
                        fld.fileReferences?.push({
                            id: Guid.create().toString() as unknown as Guid,
                            fileName: fileName,
                            originalFileName: file.name,
                            thumbnail: "",
                            contentType: file.type,
                            size: file.size,
                            created: new Date(),
                            updated: new Date(),

                            //file: file,
                            fieldId: fieldId.toString() as unknown as Guid,
                            formDataId: undefined
                        })
                        
                    }
                
                })
            })
           
        },
        saveEntity(){
            //console.log("save form template: ", JSON.stringify(this.template));
            const newEntity = this.entity?.id?.toString() === Guid.EMPTY;
            this.entity!.title = getConcatenatedTitle(this.entity as EntityData , this.entityTemplate as EntityTemplate, ' | ')
            this.entity!.description = getConcatenatedDescription(this.entity as EntityData, this.entityTemplate as EntityTemplate, ' | ')
            let api =  this.getApiRoot;//config.dataRepositoryApiRoot + "/api/entities";
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
            let fileKeys = formSubmissionstore.fileKeys as string[];
           
             //update fileReferences
             

            var formData = new FormData();
             //update fileReference
            let fileKeyIdx=0;
            attachedFiles?.forEach(file => {
                let newFileKey = this.addFileReference(file, Guid.parse(fileKeys[fileKeyIdx]));
                
                fileKeyIdx++;
            });
            

            formData.append('value', JSON.stringify(this.entity));

            attachedFiles?.forEach(file => {
                formData.append('files', file);
            });
            
            this.updatedFileKeys?.forEach(key => {
                formData.append('fileKeys', key);
            })
            
           
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
                   // router.push(`/edit-entity-editor/${this.entity!.id}`)
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
                    this.entity = data as EntityData;
                })
                .catch((error) => {
                    console.error('Load Entity API Error:', error);
                });
        },
        loadEntities(entityType: eEntityType, searchTarget: eSearchTarget, searchText: string, offset: number, max?: number){
            let api = config.dataRepositoryApiRoot + "/api/entities/"+ entityType + "/" + searchTarget + "/" + searchText + "/" +offset + "/" + max ;
            fetch(api, {
                method: 'GET'
            })
            .then(response => response.json())
            .then(data => {
                    this.entitySearchResult = data as EntitySearchResult;
            })
            .catch((error) => {
                console.error('Load Entities API Error:', error);
            });
        },
        AddToRelationObject(){
            console.log("add to relation object")
            let selectedIds = this.getSelectedEntityIds;
            console.log("selectedIds" + JSON.stringify(selectedIds))
            selectedIds.forEach((sid)=>{  
              this.entity?.subjectRelationships.push({
                subjectEntityId: sid,
                subjectEntity: this.entity,
                objectEntityId: sid,
                objectEntity: this.entity,
                name: "unlnown",
                order: 1
                })
            });
            console.log("after adding: " + JSON.stringify(this.entity))
        },
        setApiRoot(apiUrl: string){
            this.apiRoot = apiUrl;
        }
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
        },
        getSelectedEntityIds: (state)=>{
            console.log("entity editor store id: " + state.storeId)
            const entityListStore = useEntitySelectStore(state.storeId);

            return entityListStore.selectedEntityIds;
            
        },
        getApiRoot: (state) =>{
            return state.apiRoot? state.apiRoot : config.dataRepositoryApiRoot + "/api/entities";
        }
    }
});