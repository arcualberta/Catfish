import { Guid } from 'guid-typescript';
import { defineStore } from 'pinia';
import { eState } from "../../shared/constants";
import { default as config } from "@/appsettings";
//import { useLoginStore } from '@/components/login/store';
import { EntityTemplateProxy } from '@/api/entityTemplateProxy';
import { FormProxy } from '@/api/formProxy';
const formProxy = new FormProxy();
const entityTemplateProxy = new EntityTemplateProxy();
export const useEntityTemplateBuilderStore = defineStore('EntityTemplateBuilderStore', {
    state: () => ({
        id: null,
        template: null,
        formEntries: [],
        transientMessageModel: {},
        forms: [],
        apiRoot: null,
        jwtToken: {
            get: () => localStorage.getItem("catfishJwtToken"),
            set: (val) => {
                localStorage.setItem("catfishJwtToken", val);
            }
        }
    }),
    actions: {
        newTemplate() {
            this.template = {
                id: Guid.EMPTY,
                name: "New Entity Template",
                description: "Description about this new Entity Template",
                created: new Date(),
                updated: new Date(),
                state: eState.Draft,
                forms: [],
                entityTemplateSettings: {
                    metadataForms: [],
                    dataForms: [],
                    titleField: {},
                    descriptionField: {},
                    mediaField: {},
                    primaryFormId: Guid.EMPTY
                }
            };
        },
        async associateForm(formId) {
            if (formId.toString() !== Guid.EMPTY && this.forms.findIndex(form => form.id === formId) < 0) {
                var data = await formProxy.Get(formId);
                this.forms.push(data);
            }
        },
        async loadFormEntries() {
            this.formEntries = await formProxy.List();
        },
        async loadTemplate(id) {
            this.template = await entityTemplateProxy.Get(id);
            /* const api = `${this.getApiRoot}/${id}`;
             console.log("loading entityTemplate: ", api);
 
             fetch(api, {
                 method: 'GET',
                 headers: {
                     'Authorization': `bearer ${this.jwtToken}`,
                 }
             })
                 .then(response => response.json())
                 .then(data => {
                     //console.log(data);
                     this.template = data as EntityTemplate;
                 })
                 .catch((error) => {
                     console.error('Load Entity Template API Error:', error);
                 });*/
        },
        async saveTemplate() {
            const newTemplate = this.template?.id?.toString() === Guid.EMPTY;
            var response;
            if (newTemplate) {
                console.log("Saving new template.");
                if (this.template?.id?.toString() === Guid.EMPTY) {
                    this.template.id = Guid.create().toString();
                }
                response = await entityTemplateProxy.Post(this.template);
                // method = "POST";
            }
            else {
                console.log("Updating existing template.");
                response = await entityTemplateProxy.Put(this.template);
            }
            if (response) {
                this.transientMessageModel.message = "The template saved/updated successfully";
                this.transientMessageModel.messageClass = "success";
            }
            else {
                this.transientMessageModel.message = "The template fail to save/update";
                this.transientMessageModel.messageClass = "danger";
            }
        },
        deleteFormEntry(id) {
            let index = this.template.entityTemplateSettings.dataForms.findIndex(fe => fe.id === id);
            if (index >= 0)
                this.template.entityTemplateSettings.dataForms.splice(index, 1);
            else {
                index = this.template.entityTemplateSettings.metadataForms.findIndex(fe => fe.id === id);
                if (index >= 0)
                    this.template.entityTemplateSettings.metadataForms.splice(index, 1);
            }
        },
        setApiRoot(api) {
            this.apiRoot = api;
        }
    },
    getters: {
        getApiRoot(state) {
            return state.apiRoot ? state.apiRoot : config.dataRepositoryApiRoot + "/api/entity-templates";
        },
    }
});
//# sourceMappingURL=index.js.map