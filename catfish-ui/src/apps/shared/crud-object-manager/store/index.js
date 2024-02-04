import { defineStore } from 'pinia';
export const useCRUDManagerStore = defineStore('CRUDManagerStore', {
    state: () => ({
        entries: {},
        transientMessageModel: {},
        apiRoot: null
    }),
    actions: {
        loadEntries(apiUrl) {
            const api = `${apiUrl}`;
            const jwtToken = localStorage.getItem("catfishJwtToken");
            console.log('LoadApi', api);
            console.log('jwt Token', jwtToken);
            fetch(api, {
                method: 'GET',
                headers: {
                    'Authorization': `bearer ${jwtToken}`,
                }
            })
                .then(response => response.json())
                .then(data => {
                this.entries = data;
            })
                .catch((error) => {
                console.error('Listing entities API Error:', error);
            });
        },
        deleteObject(apiUrl, id) {
            const api = `${apiUrl}`;
            console.log('api', api);
            const jwtToken = localStorage.getItem("catfishJwtToken");
            fetch(api, {
                method: 'DELETE',
                headers: {
                    'Authorization': `bearer ${jwtToken}`,
                }
            })
                .then(response => {
                if (response.ok) {
                    let index = this.entries.findIndex(d => d.id === id); //find index in your array
                    this.entries.splice(index, 1); //remove element from array
                    this.transientMessageModel.message = "The entity deleted successfully";
                    this.transientMessageModel.messageClass = "success";
                }
                else {
                    this.transientMessageModel.messageClass = "danger";
                    switch (response.status) {
                        case 400:
                            this.transientMessageModel.message = "Bad request. Failed to delete the entity";
                            break;
                        case 404:
                            this.transientMessageModel.message = "Entity not found";
                            break;
                        case 500:
                            this.transientMessageModel.message = "An internal server error occurred. Failed to delete the entity";
                            break;
                        default:
                            this.transientMessageModel.message = "Unknown error occured. Failed to delete the entity";
                            break;
                    }
                }
            })
                .catch((error) => {
                this.transientMessageModel.message = "Unknown error occurred";
                this.transientMessageModel.messageClass = "danger";
                console.error('Delete entities API Error:', error);
            });
        },
        changeStatus(apiUrl, id, newStatus) {
            console.log("change state started");
            const api = `${apiUrl}`;
            console.log('api', api);
            const jwtToken = localStorage.getItem("catfishJwtToken");
            fetch(api, {
                body: JSON.stringify(newStatus),
                method: 'POST',
                headers: {
                    'encType': 'multipart/form-data',
                    'Content-Type': 'application/json',
                    'Authorization': `bearer ${jwtToken}`,
                },
            })
                .then(response => {
                if (response.ok) {
                    this.transientMessageModel.message = "The entity status changed successfully";
                    this.transientMessageModel.messageClass = "success";
                }
                else {
                    this.transientMessageModel.messageClass = "danger";
                    switch (response.status) {
                        case 400:
                            this.transientMessageModel.message = "Bad request. Failed to change state from entity";
                            break;
                        case 404:
                            this.transientMessageModel.message = "Entity not found";
                            break;
                        case 500:
                            this.transientMessageModel.message = "An internal server error occurred. Failed to change state from entity";
                            break;
                        default:
                            this.transientMessageModel.message = "Unknown error occured. Failed to change state from entity";
                            break;
                    }
                }
            })
                .catch((error) => {
                this.transientMessageModel.message = "Unknown error occurred";
                this.transientMessageModel.messageClass = "danger";
                console.error('Change State API Error:', error);
            });
        },
    }
});
//# sourceMappingURL=index.js.map