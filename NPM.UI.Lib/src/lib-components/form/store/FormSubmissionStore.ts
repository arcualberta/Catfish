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
            let array = [] as models.FileReference[];
            console.log("inside getter file Refs")
            state.form?.fields.$values
                .filter(field => helpers.getFieldType(field as models.Field) === eFieldType.AttachmentField)
                .flatMap(field => (field as models.AttachmentField).files?.$values)
                .forEach(fileRef => {
                    if (fileRef) {
                       
                        array?.push(fileRef)
                        console.log("found fileRef" + JSON.stringify(array))
                    }
                })

            return array;//as models.FileReference[];
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
           // console.log("update FileRef")
          //  console.log(file)
            const field = this.form?.fields.$values.find(fd => fd.id == fieldId);
            if (field) {
               // console.log("found the field")
                const fileRef = this.fileReferences?.find(f => f.fileName == file.name);
                if (fileRef) {
                    fileRef.fileName = file.name,
                        fileRef.originalFileName = file.name,
                        fileRef.size = file.size
                } else {

                    let fileRef: models.FileReference = {
                        id: Guid.create(), fileName: file.name, originalFileName: file.name,
                        contentType: file.type,
                        created: new Date(Date.now.toString()),
                        updated: new Date(Date.now.toString()), size: file.size, modelType: "", $type: "", thumbnail: "", cssClass: ""
                    };

                    console.log("new file ref");
                    console.log(fileRef);

                    (field as models.AttachmentField).files.$values.push(fileRef)
                   // this.fileReferences?.push(fileRef);
                   // let fileRefs = (field as models.AttachmentField).files.$values;
                    //console.log("file refs in the field")
                    //console.log(JSON.stringify(fileRefs))
                }
            }

        },
        deleteFileReference(fieldId: Guid, fileId: Guid) {

            const field = this.form?.fields.$values.find(fd => fd.id == fieldId);
            if (field) {

                const indexOfObject = (field as models.AttachmentField).files.$values.findIndex((fileRef) => {
                    return fileRef.id === fileId;
                });

                console.log("index object to be removed: "  + indexOfObject); 

                if (indexOfObject !== -1) {
                    (field as models.AttachmentField).files.$values.splice(indexOfObject, 1);
                }

                
            }
        }
    }
});