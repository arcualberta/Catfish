import { Guid } from 'guid-typescript';
import { defineStore } from 'pinia';
import { TransientMessageModel } from '../../../../components/shared/components/transient-message/models'
import { default as config } from "@/appsettings";
import { ListEntry } from '@/components/shared';
import { eState } from '../../../../components/shared/constants';

export const useCRUDManagerStore = defineStore('CRUDManagerStore', {
    state: () => ({
        entries: {} as ListEntry[] | null,
        transientMessageModel: {} as TransientMessageModel,
        apiRoot: null as string | null
    }),
    actions: {

        loadEntries(apiUrl: string) {
            const api = `${apiUrl}`
            console.log('LoadApi', api)
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
        deleteObject(apiUrl: string, id: Guid) {
            const api = `${apiUrl}`
            console.log('api', api)
            fetch(api, {
                method: 'DELETE'
            })
                .then(response => {
                    if (response.ok) {
                        let index = this.entries!.findIndex(d => d.id === id); //find index in your array
                        this.entries!.splice(index, 1);//remove element from array
                        this.transientMessageModel.message = "The form saved successfully"
                        this.transientMessageModel.messageClass = "success"
                        
                    }
                    else {
                        this.transientMessageModel.messageClass = "danger"
                        switch (response.status) {
                            case 400:
                                this.transientMessageModel.message = "Bad request. Failed to save the form";
                                break;
                            case 404:
                                this.transientMessageModel.message = "Item not found";
                                break;
                            case 500:
                                this.transientMessageModel.message = "An internal server error occurred. Failed to save the form"
                                break;
                            default:
                                this.transientMessageModel.message = "Unknown error occured. Failed to save the form"
                                break;
                        }
                    }
                 })
                .catch((error) => {
                    this.transientMessageModel.message = "Unknown error occurred"
                    this.transientMessageModel.messageClass = "danger"
                    console.error('Delete entities API Error:', error);
                });
        }, 
        changeStatus(apiUrl: string, id: Guid, newStatus: eState) {
            console.log("change state started")
            const api = `${apiUrl}`
            console.log('api', api)
            fetch(api, {
                body: JSON.stringify(newStatus),
                method: 'POST',
                headers: {
                    'encType': 'multipart/form-data',
                    'Content-Type': 'application/json'
                },
            })
                .then(response => {
                    if (response.ok) {
                        this.transientMessageModel.message = "The form status changed successfully"
                        this.transientMessageModel.messageClass = "success"
                    }
                    else {
                        this.transientMessageModel.messageClass = "danger"
                        switch (response.status) {
                            case 400:
                                this.transientMessageModel.message = "Bad request. Failed to save the form";
                                break;
                            case 404:
                                this.transientMessageModel.message = "Item not found";
                                break;
                            case 500:
                                this.transientMessageModel.message = "An internal server error occurred. Failed to save the form"
                                break;
                            default:
                                this.transientMessageModel.message = "Unknown error occured. Failed to save the form"
                                break;
                        }
                    }
                })
                .catch((error) => {
                    this.transientMessageModel.message = "Unknown error occurred"
                    this.transientMessageModel.messageClass = "danger"
                    console.error('Change State API Error:', error);
                });
        },  
       
    }
});