import { Guid } from 'guid-typescript';
import { defineStore } from 'pinia';


export const useEntityTemplateBuilderStore = defineStore('EntityTemplateBuilderStore', {
    state: () => ({
        id: null as Guid | null,
        
        
    }),
    actions: {
        newTemplate() {
            let api = `https://localhost:5020/api/entityTemplate/`;
            console.log(api)
            fetch(api, {
                method: 'POST'
            })
                .then(response => response.json())
                .then(data => {
                    console.log(data)
                    //this.form = data;
                })
                .catch((error) => {
                    console.error('create Entity Template :', error);
                });

        },
    },
    

   
});