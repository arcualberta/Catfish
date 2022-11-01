import { Guid } from 'guid-typescript';
import { defineStore } from 'pinia';

import { default as config } from "@/appsettings";
import { ListEntry } from '@/components/shared';

export const useCRUDManagerStore = defineStore('CRUDManagerStore', {
    state: () => ({
        entries: {} as ListEntry[] | null
    }),
    actions: {
        loadEntries(apiUrl: string) {
            const api = `${apiUrl}`
            fetch(api, {
                method: 'GET'
            })
                .then(response => response.json())
                .then(data => {
                    this.entries = data as ListEntry[];
                })
                .catch((error) => {
                    console.error('Listing entities API Error:', error);
                });
        },  
       
    }
});