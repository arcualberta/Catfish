import { Guid } from "guid-typescript"

import { Field, OptionFieldType, TextType, MonolingualFieldType, FieldData, Text, Form, FormData } from "../form-models";
import { getTextValue, createTextCollection, createText } from './textHelper'

/**
 * Is the given field an option field?
 * @param field
 */
export const isOptionField = (field: Field): boolean => Object.values(OptionFieldType).map(x => x as unknown as string).includes(field.type as unknown as string);

/**
 * Is the given field a multilingual text field?
 * @param field
 */
export const isMultilingualTextInputField = (field: Field): boolean => Object.values(TextType).map(x => x as unknown as string).includes(field.type as unknown as string);

/**
 * Is the given field a monolingual text field?
 * @param field
 */
export const isMonolingualTextInputField = (field: Field): boolean => Object.values(MonolingualFieldType).map(x => x as unknown as string).includes(field.type as unknown as string);

/**
 * Is the given field a text input field (i.e. monolingual or multilingual)?
 * @param field
 */
export const isTextInputField = (field: Field): boolean => Object.values(MonolingualFieldType).map(x => x as unknown as string).includes(field.type as unknown as string);

/**
 * Returns the title of a field as a string. If multiple values are specified, only returns the first value.
 * @param field: input field
 * @param lang: optional language. If not specified, returns the value in the first language.
 */
export const getFieldTitle = (field: Field, lang: string | null): string | null => {
    const txtVals = getTextValue(field.title, lang);
    return txtVals?.length > 0 ? txtVals[0] : null;
}

/**
 * Returns the description of a field as a string. If multiple values are specified, only returns the first value.
 * @param field: input field
 * @param lang: optional language. If not specified, returns the value in the first language.
 */
export const getFieldDescription = (field: Field, lang: string | null): string | null => {
    const txtVals = getTextValue(field.description, lang);
    return txtVals?.length > 0 ? txtVals[0] : null;
}

/**
 * Creates and returns a FieldData object for a given field. This function should be called by implementation of form-submission 
 * functionality to create objects to store values submitted by users for form fields.
 * 
 * @param field
 * @param lang Language or languages to be supported.
 */
export const createFieldData = (field: Field, lang: string[] | string): FieldData => {
    const fieldData = {
        id: Guid.create().toString() as unknown as Guid,
        fieldId: field.id
    } as FieldData;

    if (isOptionField(field)) {
        fieldData.selectedOptionIds = []

        if (field.allowCustomOptionValues)
            fieldData.customOptionValues = []

        if (field.options?.find(opt => opt.isExtendedInput))
            fieldData.extendedOptionValues = []
    }
    else if (isMultilingualTextInputField(field)) {
        const languages = typeof(lang) == 'string' ? [lang] : lang
        fieldData.multilingualTextValues = [createTextCollection(languages)]
    }
    else if (isMonolingualTextInputField(field)) {
        fieldData.monolingualTextValues = [createText(null)]
    }

    return fieldData
}
export const createFormData = (form: Form, lang: string | string[]): FormData => {
    const formData = {
        id: Guid.EMPTY as unknown as Guid,
        formId: form.id,
        fieldData: []
    } as unknown as FormData;

    form.fields?.forEach(field => {
        const fieldData = createFieldData(field, lang)
        formData.fieldData.push(fieldData)
    })

    return formData
}