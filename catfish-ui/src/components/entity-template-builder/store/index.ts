import { Guid } from 'guid-typescript';
import { defineStore } from 'pinia';
import { entityTemplate, formEntry } from '../models';


export const useEntityTemplateBuilderStore = defineStore('EntityTemplateBuilderStore', {
    state: () => ({
        id: null as Guid | null,
        template: null as entityTemplate | null
        
    }),
    actions: {
        newTemplate() {
            this.template =  {
                id: Guid.EMPTY as unknown as Guid,
                created: new Date(),
                updated: new Date(),
                name: "New Entity Template",
                description: "Description about this new Entity Template",
                state: null,
                metadataForms: [] as formEntry[],
                dataForms: [] as formEntry[]
            };

        },
    },
    

   
});