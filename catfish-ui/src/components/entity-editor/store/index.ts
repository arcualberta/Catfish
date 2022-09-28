import { Guid } from 'guid-typescript';
import { defineStore } from 'pinia';
import { Entity, TemplateEntry } from '../models';
import { EntityTemplate } from '../../entity-template-builder/models'
import { default as config } from "@/appsettings";
import { eEntityType } from '@/components/shared/constants';
import { createFormData } from '@/components/shared/form-helpers'
import { Form, FormData } from '@/components/shared/form-models'
import { FORMERR } from 'dns';

export const useEntityEditorStore = defineStore('EntityEditorStore', {
    state: () => ({
        id: null as Guid | null,
        templates: [] as TemplateEntry[],
        entityTemplate: null as EntityTemplate | null,
        entity: null as Entity | null

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
                data: [] as FormData[]
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
        instantiateEntityFormData() {
            //Instantiating FormData objects for primary metadata forms
            this.entityTemplate?.entityTemplateSettings?.metadataForms?.filter(form => form.isPrimary).forEach(formEntry => {
                if (this.entity!.data.filter(formData => formData.formId == formEntry.formId).length == 0) {
                    const form = this.entityTemplate!.forms!.filter(f => f.id == formEntry.formId)[0] as Form;
                    const formData = createFormData(form!, "");
                    formData.id = Guid.create().toString() as unknown as Guid;
                    this.entity!.data.push(formData)
                }
            })

            //Instantiating FormData objects for primary data forms
            this.entityTemplate?.entityTemplateSettings?.dataForms?.filter(form => form.isPrimary).forEach(formEntry => {
                if (this.entity!.data.filter(formData => formData.formId == formEntry.formId).length == 0) {
                    const form = this.entityTemplate!.forms!.filter(f => f.id == formEntry.formId)[0] as Form;
                    const formData = createFormData(form!, "");
                    formData.id = Guid.create().toString() as unknown as Guid;
                    this.entity!.data.push(formData)
                }
            })
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
        }
    }
});