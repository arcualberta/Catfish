import { Guid } from 'guid-typescript';
import { defineStore } from 'pinia';
import { EntityTemplate, FormEntry } from '../models';


export const useEntityTemplateBuilderStore = defineStore('EntityTemplateBuilderStore', {
    state: () => ({
        id: null as Guid | null,
        template: null as EntityTemplate | null
        
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
                forms: [],
                metadataForms: [{name: "Metadata Template",
                    formId: Guid.create()}],
                dataForms: [{name: "Data Template",
                formId: Guid.create()}]
            };

        },
    },
    

   
});