import { defineStore } from 'pinia';

import { Guid } from "guid-typescript";

import { Form, Field, FieldType, OptionFieldType, TextCollection } from '../../shared/form-models'

import { createOption, createTextCollection, isOptionField } from '../../shared/form-helpers'

export const useFormEditorStore = defineStore('FormEditorStore', {
    state: () => ({
        lang: ["en", "fr"],
        form: null as Form | null,
        transientMessage: null as string | null,
        transientMessageClass: null as string | null
    }),
    getters: {
/*
        textModels: (state): models.Text[] => {

            //Handling multilingual fields
            const array = state.form?.fields.$values
                .filter(field => helpers.getFieldType(field as models.Field) === eFieldType.TextField
                    || helpers.getFieldType(field as models.Field) === eFieldType.TextArea)
                .flatMap(textField => (textField as models.MultilingualTextField).values?.$values)
                .flatMap(multilingualValue => multilingualValue?.values.$values)

            //Handling monolingual fields
            state.form?.fields.$values
                .filter(field => helpers.getFieldType(field as models.Field) === eFieldType.DateField
                    || helpers.getFieldType(field as models.Field) === eFieldType.DecimalField
                    || helpers.getFieldType(field as models.Field) === eFieldType.EmailField
                    || helpers.getFieldType(field as models.Field) === eFieldType.IntegerField
                    || helpers.getFieldType(field as models.Field) === eFieldType.MonolingualTextField)
                .flatMap(textField => (textField as models.MonolingualTextField).values?.$values)
                .forEach(txt => array?.push(txt))

            return array as models.Text[];
        },
        optionModels: (state): models.Option[] => {
            const array = state.form?.fields.$values
                .filter(field => helpers.getFieldType(field as models.Field) === eFieldType.CheckboxField
                    || helpers.getFieldType(field as models.Field) === eFieldType.RadioField
                    || helpers.getFieldType(field as models.Field) === eFieldType.SelectField)
                .flatMap(optionsField => (optionsField as models.OptionsField).options?.$values)

            return array as models.Option[];
        },
        fileReferences: (state): models.FileReference[] => {

            return helpers.getFileReferences(state.form as models.FieldContainer);

            //return state.form?.fields.$values
            //    .filter(field => helpers.getFieldType(field as models.Field) === eFieldType.AttachmentField)
            //    .flatMap(field => (field as models.AttachmentField).files?.$values) as models.FileReference[];
        }
*/
    },
    actions: {
        newForm() {
            this.form = {
                id: Guid.EMPTY as unknown as Guid,
                name: createTextCollection(this.lang),
                description: createTextCollection(this.lang),
                fields: [] as Field[]
            } as Form;
        },
        newField(fieldType: FieldType) {

            const field = {
                id: Guid.create().toString() as unknown as Guid,
                title: createTextCollection(this.lang),
                description: createTextCollection(this.lang),
                type: fieldType,
            } as unknown as Field;

            if (isOptionField(field)) {
                field.options = [createOption(this.lang)]
            }

            this.form?.fields.push(field); 
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
        
/*
        setTextValue(id: Guid, value: string) {
            const txt = this.textModels.find(field => field.id === id);
            if (txt)
                txt.value = value;
        },
        setOptionSelection(id: Guid, selected: boolean) {
            const option = this.optionModels.find(field => field.id === id);
            if (option)
                option.selected = selected;
        },
        addExtendedOptionValue(id: Guid, value: string) {
            const option = this.optionModels.find(field => field.id === id);
            if (option) {
                if (!option.extendedValues.$values)
                    option.extendedValues.$values = [];
                option.extendedValues.$values.push(value);
                console.log(JSON.stringify(option.extendedValues.$values))
            }
        },
        removeExtendedOptionValue(id: Guid, index: number) {
            const option = this.optionModels.find(field => field.id === id);
            if (option) {
                option.extendedValues.$values.splice(index, 1);
            }
        },
        updateFileReference(fieldId: Guid, file: File) {

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

                    (field as models.AttachmentField).files.$values.push(fileRef)
                }
            }

        },
        deleteFileReference(fieldId: Guid, fileId: Guid) {

            const field = this.form?.fields.$values.find(fd => fd.id == fieldId);
            if (field) {

                const indexOfObject = (field as models.AttachmentField).files.$values.findIndex((fileRef) => {
                    return fileRef.id === fileId;
                });

                // console.log("index object to be removed: "  + indexOfObject); 

                if (indexOfObject !== -1) {
                    (field as models.AttachmentField).files.$values.splice(indexOfObject, 1);
                }


            }
        },
        appendMonolingualValue(target: models.MonolingualTextField) {
            const newText = helpers.createTextElement();
            target.values?.$values.push(newText);
        },
        removeMonolingualValue(target: models.MonolingualTextField, id: Guid) {
            const index = target.values?.$values.findIndex(txt => txt.id === id) as number;
            if (index >= 0)
                target.values?.$values.splice(index, 1);
        },
        appendMutilingualValue(target: models.MultilingualTextField) {
            const languages = target.values?.$values[0] ? helpers.getLanguages(target.values?.$values[0]) : ["en"];
            const newMultilingualValue = helpers.createMultilingualValueElment(languages);

            target.values?.$values.push(newMultilingualValue);
        },
        removeMutilingualValue(target: models.MultilingualTextField, id: Guid) {
            const index = target.values?.$values.findIndex(txtCollection => txtCollection.id === id) as number;
            if (index >= 0)
                target.values?.$values.splice(index, 1);
        },
*/
    }
});