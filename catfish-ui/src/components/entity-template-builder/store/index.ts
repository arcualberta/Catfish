import { Guid } from 'guid-typescript';
import { defineStore } from 'pinia';
import { EntityTemplate, FormEntry } from '../models';
import { eState } from "../../shared/constants";
import { default as config } from "@/appsettings";


export const useEntityTemplateBuilderStore = defineStore('EntityTemplateBuilderStore', {
    state: () => ({
        id: null as Guid | null,
        template: null as EntityTemplate | null,
        formEntries: [] as FormEntry[]

    }),
    actions: {
        newTemplate() {
            this.template = {
                id: Guid.EMPTY as unknown as Guid,
                name: "New Entity Template",
                description: "Description about this new Entity Template",
                created: new Date(),
                updated: null,
                state: eState.Unpublished,
                forms: [],
                metadataForms: [],
                dataForms: []
            };

        },
        loadForms() {
            const api = `${config.dataRepositoryApiRoot}/api/forms`;
            console.log("loading forms: ", api);

            fetch(api, {
                method: 'GET'
            })
                .then(response => response.json())
                .then(data => {
                    this.formEntries = data as FormEntry[]
                })
                .catch((error) => {
                    console.error('Load Form API Error:', error);
                });
        },
        saveTemplate(){
            console.log("save form template: ", JSON.stringify(this.template));
            const newTemplate = this.template?.id?.toString() === Guid.EMPTY;
            let api = "${config.dataRepositoryApiRoot}/api/entityTemplate";
            let method = "";
            if (newTemplate) {
                console.log("Saving new template.");
                if(this.template?.id?.toString() === Guid.EMPTY)
                    this.template.id = Guid.create().toString() as unknown as Guid;
                method = "POST";
            }
            else {
                console.log("Updating existing template.")
                api = `${api}/${this.template?.id}`
                method = "PUT";
            }
            fetch(api, {
                method: method
            })
            .then(response => response.json())
            .then(data => {
                    console.log(data);
                    alert("save successful")
            })
            .catch((error) => {
                if(method === 'POST'){
                   if(this.template?.id)
                    this.template.id = Guid.EMPTY as unknown as Guid;
                }
                console.error('Save/Update Entity Template API Error:', error);
            });
        }
    },    
});