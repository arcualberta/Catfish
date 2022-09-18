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
            console.log("loading forms: ", api)

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
        }
    },    
});