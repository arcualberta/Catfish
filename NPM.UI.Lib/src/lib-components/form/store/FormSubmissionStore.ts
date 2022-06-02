import { defineStore } from 'pinia';
import { Guid } from 'guid-typescript';

import * as models from '../models'
import * as helpers from '../helpers'
import { eFieldType/*, eDataElementType*/ } from '../enumerations'


export const useFormSubmissionStore = defineStore('FormSubmissionStore', {
    state: () => ({
        form: null as models.FieldContainer | null,
    }),
    getters: {
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
          
            return state.form?.fields.$values
                .filter(field => helpers.getFieldType(field as models.Field) === eFieldType.AttachmentField)
                .flatMap(field => (field as models.AttachmentField).files?.$values) as models.FileReference[];

        }
    },
    actions: {
        setTextValue(id: Guid, value: string) {
            const field = this.textModels.find(field => field.id === id);
            if (field)
                field.value = value;
        },
        setOptionSelection(id: Guid, selected: boolean) {
            const field = this.optionModels.find(field => field.id === id);
            if (field)
                field.selected = selected;
        },
        updateFileReference(fieldId: Guid, file: File) {
         
            const field = this.form?.fields.$values.find(fd => fd.id == fieldId);
            if (field) {

                const fileRef = this.fileReferences?.find(f => f.fileName == file.name && !f.id);
                if (fileRef) {
                    fileRef.fileName = file.name,
                        fileRef.originalFileName = file.name,
                        fileRef.size = file.size
                } else {

                    const fileRef: models.FileReference = {
                        id: Guid.create(), fileName: file.name, originalFileName: file.name,
                        contentType: file.type,
                        created: new Date(Date.now.toString()),
                        updated: new Date(Date.now.toString()), size: file.size, modelType: "", $type: "", thumbnail: "", cssClass: ""
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
        }
    }
});