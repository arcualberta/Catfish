import { defineStore } from 'pinia';

import { Guid } from "guid-typescript";

import { Form, FormData } from '../../shared/form-models'
import { createFormData } from '../../shared/form-helpers'

export const useFormSubmissionStore = defineStore('FormSubmissionStore', {
    state: () => ({
        lang: "en",
        form: null as Form | null,
        formData: {} as FormData,
        transientMessage: null as string | null,
        transientMessageClass: null as string | null
    }),
    actions: {
        loadForm(id: Guid) {
            let api = `https://localhost:5020/api/forms/${id}`;
            console.log(api)
            fetch(api, {
                method: 'GET'
            })
                .then(response => response.json())
                .then(data => {
                    this.form = data
                    this.formData = createFormData(this.form as Form, this.lang)
                })
                .catch((error) => {
                    console.error('Load Form API Error:', error);
                });

        },
        submitForm() {
            console.log("submitForm")
        },
        saveForm() {
            if (!this.form) {
                console.error("Cannot save null form.")
                return;
            }

            const newForm = this.form?.id?.toString() === Guid.EMPTY;
            let api = "https://localhost:5020/api/forms";
            let method = "";
            if (newForm) {
                console.log("Saving new form.")
                this.form.id = Guid.create().toString() as unknown as Guid;
                method = "POST";
            }
            else {
                console.log("Updating existing form.")
                api = `${api}/${this.form.id}`
                method = "PUT";
            }

            fetch(api,
                {
                    body: JSON.stringify(this.form),
                    method: method,
                    headers: {
                        'encType': 'multipart/form-data',
                        'Content-Type': 'application/json'
                    },
                })
                .then(response => {
                    if (response.ok) {
                        this.transientMessage = "The form saved successfully"
                        this.transientMessageClass = "success"
                    }
                    else {
                        this.transientMessageClass = "danger"
                        switch (response.status) {
                            case 400:
                                this.transientMessage = "Bad request. Failed to save the form";
                                break;
                            case 404:
                                this.transientMessage = "Form not found";
                                break;
                            case 500:
                                this.transientMessage = "An internal server error occurred. Failed to save the form"
                                break;
                            default:
                                this.transientMessage = "Unknown error occured. Failed to save the form"
                                break;
                        }
                    }
                })
                .catch((error) => {
                    this.transientMessage = "Unknown error occurred"
                    this.transientMessageClass = "danger"
                    console.error('Form Save API Error:', error)
                });
        },
        clearMessages() {
            this.transientMessage = null;
        },
    }
});