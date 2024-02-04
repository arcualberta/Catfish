import { defineStore } from 'pinia';
import { Guid } from "guid-typescript";
import { default as config } from "@/appsettings";
import { createFormData } from '../../shared/form-helpers';
import { eState } from '@/components/shared/constants';
import { FormDataProxy } from '@/api/formDataProxy';
const proxy = new FormDataProxy();
export const useFormSubmissionStore = defineStore('FormSubmissionStore', {
    state: () => ({
        lang: "en",
        form: null,
        formData: {},
        transientMessageModel: {},
        transientMessage: null,
        transientMessageClass: null,
        files: [],
        fileKeys: []
    }),
    actions: {
        loadForm(id, retainCurrentFormData) {
            let api = `${config.dataRepositoryApiRoot}/api/forms/${id}`; //`https://localhost:5020/api/forms/${id}`;
            console.log(api);
            fetch(api, {
                method: 'GET'
            })
                .then(response => response.json())
                .then(data => {
                this.form = data;
                if (!retainCurrentFormData)
                    this.formData = createFormData(this.form, this.lang);
            })
                .catch((error) => {
                console.error('Load Form API Error:', error);
            });
        },
        loadSubmission(id) {
            let api = `${config.dataRepositoryApiRoot}/api/form-submissions/${id}`; //`https://localhost:5020/api/form-submissions/${id}`;
            console.log(api);
            fetch(api, {
                method: 'GET'
            })
                .then(response => response.json())
                .then(data => {
                this.formData = data;
                if (this.formData?.formId)
                    this.loadForm(this.formData.formId, true);
            })
                .catch((error) => {
                console.error('Load Form API Error:', error);
            });
        },
        validateFormData() {
            console.log("TODO: Validate form data.");
            return true;
        },
        async submitForm() {
            if (!this.validateFormData()) {
                console.log("Form validation failed.");
                return false;
            }
            const isNewForm = this.formData?.id?.toString() === Guid.EMPTY;
            var responseStatus;
            if (isNewForm) {
                this.formData.state = eState.Draft;
                responseStatus = await proxy.Post(this.formData);
            }
            else {
                responseStatus = await proxy.Put(this.formData);
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
                     body: JSON.stringify(this.formData),
                     method: method,
                     headers: {
                         'encType': 'multipart/form-data',
                         'Content-Type': 'application/json'
                     },
                 })
                 .then(async response => {
                     if (response.ok) {
                         if (newForm) {
                             const id = await response.json();
                             this.formData.id = id as unknown as Guid;
                         }
                         this.transientMessage = "Success";
                         this.transientMessageClass = "success";
                         console.log("Form submission successfull.");
                     }
                     else {
                         this.transientMessageClass = "danger"
                         switch (response.status) {
                             case 400:
                                 this.transientMessage = "Bad request. Failed to submit the form";
                                 break;
                             case 404:
                                 this.transientMessage = "Form submission not found";
                                 break;
                             case 500:
                                 this.transientMessage = "An internal server error occurred. Failed to submit the form"
                                 break;
                             default:
                                 this.transientMessage = "Unknown error occured. Failed to submit the form"
                                 break;
                         }
                     }
                 })
                 .catch((error) => {
                     if (newForm && this.formData)
                         this.formData.id = Guid.EMPTY as unknown as Guid;
 
                     this.transientMessage = "Unknown error occurred"
                     this.transientMessageClass = "danger"
                     console.error('FormData Submit API Error:', error)
                 });*/
        },
        saveForm() {
            if (!this.form) {
                console.error("Cannot save null form.");
                return;
            }
            const newForm = this.form?.id?.toString() === Guid.EMPTY;
            let api = `${config.dataRepositoryApiRoot}/api/forms`;
            let method = "";
            if (newForm) {
                console.log("Saving new form.");
                this.form.id = Guid.create().toString();
                method = "POST";
                this.form.state = eState.Draft;
            }
            else {
                console.log("Updating existing form.");
                api = `${api}/${this.form.id}`;
                method = "PUT";
            }
            fetch(api, {
                body: JSON.stringify(this.form),
                method: method,
                headers: {
                    'encType': 'multipart/form-data',
                    'Content-Type': 'application/json'
                },
            })
                .then(response => {
                if (response.ok) {
                    this.transientMessage = "The form saved successfully";
                    this.transientMessageClass = "success";
                }
                else {
                    this.transientMessageClass = "danger";
                    switch (response.status) {
                        case 400:
                            this.transientMessage = "Bad request. Failed to save the form";
                            break;
                        case 404:
                            this.transientMessage = "Form not found";
                            break;
                        case 500:
                            this.transientMessage = "An internal server error occurred. Failed to save the form";
                            break;
                        default:
                            this.transientMessage = "Unknown error occured. Failed to save the form";
                            break;
                    }
                }
            })
                .catch((error) => {
                this.transientMessage = "Unknown error occurred";
                this.transientMessageClass = "danger";
                console.error('Form Save API Error:', error);
            });
        },
        clearMessages() {
            this.transientMessage = null;
        },
        addFile(file, fieldId) {
            this.files?.push(file);
            this.fileKeys?.push(fieldId.toString());
        },
        putFile(files, fieldId) {
            Array.from(files).forEach(file => {
                console.log("fieldId:" + fieldId);
                this.addFile(file, fieldId);
            });
        },
    }
});
//# sourceMappingURL=index.js.map