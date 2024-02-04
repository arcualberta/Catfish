import { defineStore } from 'pinia';
import { Guid } from "guid-typescript";
import { default as config } from "@/appsettings";
import { eState } from '@/components/shared/constants';
import { FormProxy } from '@/api/formProxy';
const proxy = new FormProxy();
export const useFormBuilderStore = defineStore('FormBuilderStore', {
    state: () => ({
        lang: ["en", "fr"],
        form: null,
        transientMessageModel: {},
        apiRoot: null,
        activeContainer: null,
        jwtToken: {
            get: () => localStorage.getItem("catfishJwtToken"),
            set: (val) => {
                localStorage.setItem("catfishJwtToken", val);
            }
        }
    }),
    actions: {
        createNewForm() {
            this.form = {
                id: Guid.EMPTY,
                name: "",
                description: "",
                fields: [],
                status: eState.Draft
            };
        },
        async loadForm(id) {
            proxy.setSecurityToken(this.jwtToken);
            this.form = await proxy.Get(id);
        },
        async saveForm() {
            if (!this.form) {
                console.error("Cannot save a null form.");
                return false;
            }
            const newForm = this.form?.id?.toString() === Guid.EMPTY;
            var responseStatus;
            proxy.setSecurityToken(this.jwtToken);
            if (newForm) {
                this.form.id = Guid.create().toString();
                this.form.status = eState.Draft;
                responseStatus = await proxy.Post(this.form);
            }
            else {
                responseStatus = await proxy.Put(this.form);
            }
            if (responseStatus) {
                this.transientMessageModel.message = "The template saved/updated successfully";
                this.transientMessageModel.messageClass = "success";
            }
            else {
                this.transientMessageModel.message = "The template fail to save/update";
                this.transientMessageModel.messageClass = "danger";
            }
            return responseStatus;
            /* fetch(api,
                 {
                     body: JSON.stringify(this.form),
                     method: method,
                     headers: {
                         'encType': 'multipart/form-data',
                         'Content-Type': 'application/json',
                         'Access-Control-Allow-Origin': `${config.dataRepositoryApiRoot}`,//'http://localhost:5020',
                         'Access-Control-Allow-Credentials': 'true',
                         'Authorization': `bearer ${this.jwtToken}`
                     },
                    
                 })
                 .then(response => {
                     if (response.ok) {
                         this.transientMessageModel.message = "The form saved successfully"
                         this.transientMessageModel.messageClass = "success"
                     }
                     else {
                         if (newForm && this.form)
                             this.form.id = Guid.EMPTY as unknown as Guid;
 
                         this.transientMessageModel.messageClass = "danger"
                         switch (response.status) {
                             case 400:
                                 this.transientMessageModel.message = "Bad request. Failed to save the form";
                                 break;
                             case 404:
                                 this.transientMessageModel.message = "Form not found";
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
                     if (newForm && this.form)
                         this.form.id = Guid.EMPTY as unknown as Guid;
 
                     this.transientMessageModel.message = "Unknown error occurred"
                     this.transientMessageModel.messageClass = "danger"
                     console.error('Form Save API Error:', error)
                 });
                 */
        },
        setApiRoot(api) {
            this.apiRoot = api;
        }
    },
    getters: {
        getApiRoot(state) {
            return state.apiRoot ? state.apiRoot : config.dataRepositoryApiRoot + "/api/forms";
        }
    }
});
//# sourceMappingURL=index.js.map