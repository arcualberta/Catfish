import AttachmentField from "@/components/form-submission/components/AttachmentField.vue";
import { Guid } from "guid-typescript";
import { OptionFieldType, TextType, MonolingualFieldType, CompositeFieldType } from "../form-models";
import { getTextValue, createTextCollection, createText } from './textHelper';
/**
 * Is the given field an option field?
 * @param field
 */
export const isOptionField = (field) => Object.values(OptionFieldType).map(x => x).includes(field.type);
/**
 * Is the given field a multilingual text field?
 * @param field
 */
export const isMultilingualTextInputField = (field) => Object.values(TextType).map(x => x).includes(field.type);
/**
 * Is the given field a monolingual text field?
 * @param field
 */
export const isMonolingualTextInputField = (field) => Object.values(MonolingualFieldType).map(x => x).includes(field.type);
/**
 * Is the given field a text input field (i.e. monolingual or multilingual)?
 * @param field
 */
export const isTextInputField = (field) => Object.values(MonolingualFieldType).map(x => x).includes(field.type);
export const isAttachmentField = (field) => Object.values(AttachmentField).map(x => x).includes(field.type);
/**
 * Returns the title of a field as a string. If multiple values are specified, only returns the first value.
 * @param field: input field
 * @param lang: optional language. If not specified, returns the value in the first language.
 */
export const getFieldTitle = (field, lang) => {
    const txtVals = getTextValue(field.title, lang);
    return txtVals?.length > 0 ? txtVals[0] : null;
};
/**
 * Returns the description of a field as a string. If multiple values are specified, only returns the first value.
 * @param field: input field
 * @param lang: optional language. If not specified, returns the value in the first language.
 */
export const getFieldDescription = (field, lang) => {
    const txtVals = getTextValue(field.description, lang);
    return txtVals?.length > 0 ? txtVals[0] : null;
};
/**
 * Creates and returns a FieldData object for a given field. This function should be called by implementation of form-submission
 * functionality to create objects to store values submitted by users for form fields.
 *
 * @param field
 * @param lang Language or languages to be supported.
 */
export const createFieldData = (field, lang) => {
    const fieldData = {
        id: Guid.create().toString(),
        fieldId: field.id
    };
    if (isOptionField(field)) {
        fieldData.selectedOptionIds = [];
        if (field.allowCustomOptionValues)
            fieldData.customOptionValues = [];
        if (field.options?.find(opt => opt.isExtendedInput))
            fieldData.extendedOptionValues = [];
    }
    else if (isAttachmentField(field)) {
        fieldData.fileReferences = [];
        if (field.allowCustomOptionValues)
            fieldData.customOptionValues = [];
        if (field.options?.find(opt => opt.isExtendedInput))
            fieldData.extendedOptionValues = [];
    }
    else if (isMultilingualTextInputField(field)) {
        const languages = typeof (lang) == 'string' ? [lang] : lang;
        fieldData.multilingualTextValues = [createTextCollection(languages)];
    }
    else if (isMonolingualTextInputField(field)) {
        fieldData.monolingualTextValues = [createText(null)];
    }
    else if (isCompositeField(field)) {
        fieldData.compositeFieldData = [];
        //createCompositeFieldData(field, lang);
        field.fields?.forEach((fld) => {
            let fldData = createCompositeFieldData(fld, lang);
            fieldData.compositeFieldData?.push(fldData);
        });
    }
    return fieldData;
};
export const createCompositeFieldData = (field, lang) => {
    if (!lang)
        lang = "en";
    return createFieldData(field, lang);
};
export const createFormData = (form, lang) => {
    const formData = {
        id: Guid.EMPTY,
        formId: form.id,
        fieldData: []
    };
    form.fields?.forEach(field => {
        const fieldData = createFieldData(field, lang);
        formData.fieldData.push(fieldData);
    });
    return formData;
};
export const isCompositeField = (field) => Object.values(CompositeFieldType).map(x => x).includes(field.type);
//# sourceMappingURL=fieldHelper.js.map