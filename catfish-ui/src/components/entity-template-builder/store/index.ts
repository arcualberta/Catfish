import { Guid } from 'guid-typescript';
import { defineStore } from 'pinia';
import { EntityTemplate, FormEntry } from '../models';
import { eState } from "../../shared/constants";


export const useEntityTemplateBuilderStore = defineStore('EntityTemplateBuilderStore', {
    state: () => ({
        id: null as Guid | null,
        template: null as EntityTemplate | null
        
    }),
    actions: {
        newTemplate() {
            this.template =  {
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
    },
    

   
});