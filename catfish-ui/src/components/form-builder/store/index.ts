import { defineStore } from 'pinia';
import { Guid } from "guid-typescript";
import { default as config } from "@/appsettings";
import { Field, FieldType, OptionFieldType, TextCollection, Option } from '../../shared/form-models'
import { createOption, createTextCollection, isOptionField, cloneTextCollection } from '../../shared/form-helpers'
import { TransientMessageModel } from '../../shared/components/transient-message/models'
import { FormTemplate } from '@/components/shared/form-models/formTemplate';

export const useFormBuilderStore = defineStore('FormBuilderStore', {
    state: () => ({
        lang: ["en", "fr"],
        form: null as FormTemplate | null,
        transientMessageModel: {} as TransientMessageModel
    }),
    actions: {
        loadForm(id: Guid) {
            let api = `${config.dataRepositoryApiRoot}/api/forms/${id}`;//`https://localhost:5020/api/forms/${id}`;
            console.log(api)
            fetch(api, {
                method: 'GET'
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

            const newForm = this.form?.id?.toString() === Guid.EMPTY;
            let api = `${config.dataRepositoryApiRoot}/api/forms`//"https://localhost:5020/api/forms";
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
                        'Content-Type': 'application/json',
                        'Access-Control-Allow-Origin': `${config.dataRepositoryApiRoot}`,//'http://localhost:5020',
                        'Access-Control-Allow-Credentials': 'true'
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
        /*updateFileReference(fieldId: Guid, file: File) {
         
            const field = this.form?.fields.$values.find(fd => fd.id == fieldId);
            if (field) {

                const fileRef = this.fileReferences?.find(f => f.fileName == file.name && !f.id);
                if (fileRef) {
                    fileRef.fileName = file.name;
                    fileRef.originalFileName = file.name;
                    fileRef.size = file.size;
                    fileRef.file = file;
                    fileRef.fieldId = field.id as Guid;
                }
                else {

                    const fileRef: models.FileReference = {
                        id: Guid.create().toString() as unknown as Guid,
                        fileName: file.name,
                        fieldId: field.id as Guid,
                        originalFileName: file.name,
                        contentType: file.type,
                        created: new Date(),
                        updated: new Date(Date.now.toString()),
                        size: file.size,
                        modelType: "Catfish.Core.Models.Contents.FileReference",
                        $type: "Catfish.Core.Models.Contents.FileReference",
                        file: file,
                        thumbnail: "",
                        cssClass: ""
                    };

                    field.files?.push(fileRef)
                }
            }

        },*/
    }
});