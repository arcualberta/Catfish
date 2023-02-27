import { defineStore } from 'pinia';
import { Guid } from "guid-typescript";
import { default as config } from "@/appsettings";
import { Field, FieldType, OptionFieldType, TextCollection, Option } from '../../shared/form-models'
import { createOption, createTextCollection, isOptionField, cloneTextCollection } from '../../shared/form-helpers'
import { TransientMessageModel } from '../../shared/components/transient-message/models'
import { FormTemplate } from '@/components/shared/form-models/formTemplate';
import { eState } from '@/components/shared/constants';

export const useFormBuilderStore = defineStore('FormBuilderStore', {
    state: () => ({
        lang: ["en", "fr"],
        form: null as FormTemplate | null,
        transientMessageModel: {} as TransientMessageModel,
        apiRoot: null as string |null,
        activeContainer: null as FormTemplate | null as Field | null,
        jwtToken: {
            get: () =>  localStorage.getItem("catfishJwtToken"),
            set:(val: string) => {
                localStorage.setItem("catfishJwtToken", val)
            }
        } as unknown as string
    }),
    actions: {
        createNewForm(){
            this.form = {
                id: Guid.EMPTY as unknown as Guid,
                name: "",
                description: "",
                fields: [] as Field[],
                status: eState.Draft
            };
        },
        loadForm(id: Guid) {
            // //this.getApiRoot => localhost:40520/api/forms
            let api = `${this.getApiRoot}/${id}`;//`https://localhost:5020/api/forms/${id}`;
            console.log(api)
            fetch(api, {
                method: 'GET',
                headers: {
                    'Authorization': `bearer ${this.jwtToken}`,
                }
            })
                .then(response => response.json())
                .then(data => {
                    this.form = data;
                })
                .catch((error) => {
                    console.error('Load Form API Error:', error);
                });

        },
        saveForm() {
            if (!this.form) {
                console.error("Cannot save null form.")
                return;
            }
          //  console.log("save form jwt " + this.jwtToken)
            const newForm = this.form?.id?.toString() === Guid.EMPTY;
            let api = `${this.getApiRoot}`;//`${config.dataRepositoryApiRoot}/api/forms`//"https://localhost:5020/api/forms";
            let method = "";
            if (newForm) {
                console.log("Saving new form.")
                this.form.id = Guid.create().toString() as unknown as Guid;
                this.form.status= eState.Draft;
                method = "POST";
                console.log(JSON.stringify(this.form))
            }
            else {
                console.log("Updating existing form.")
                api = `${api}/${this.form.id}`
                method = "PUT";
                console.log("form ", JSON.stringify(this.form))
            }

            fetch(api,
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
        },
        setApiRoot(api: string){
            this.apiRoot = api;
        }
        
    },
    getters:{
        getApiRoot(state){
            return state.apiRoot? state.apiRoot : config.dataRepositoryApiRoot + "/api/forms";
        }
    } 
});