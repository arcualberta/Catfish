import { MonolingualTextField, MultilingualTextField, OptionsField } from '../models/fieldContainer'

export enum FieldValidationStatus {
    VALID = 'VALID',
    VALUE_REQUIRED = 'VALUE_REQUIRED',
    VALUE_INVALID = 'VALUE_INVALID'
}


export function validateMultilingualTextField(field: MultilingualTextField): FieldValidationStatus {
    //If the field itself is not a required field, any contents in inner fields 
    //will be valid, including none
    if (!field.required)
        return FieldValidationStatus.VALID;

    //We are here means, this is a required field. This means, we need to make sure
    //the field (which can potentially have multiple values) has at least one value
    //in at least one language.
    let valueFound = false;
    for (let i = 0; !valueFound && field?.values && (i < field.values?.$values?.length); ++i) {
        const txtCollection = field?.values?.$values[i];
        for (let k = 0; !valueFound && txtCollection.values && (k < txtCollection.values?.$values.length); ++k) {
            valueFound = txtCollection.values?.$values[k]?.value?.trim().length > 0;
        }
    }

    //Validation is successful as long as some value is in an inner field.
    return valueFound ? FieldValidationStatus.VALID : FieldValidationStatus.VALUE_REQUIRED;
}

export function validateMonolingualTextField(field: MonolingualTextField): FieldValidationStatus {
    //If the field itself is not a required field, any contents in inner fields 
    //will be valid, including none
    if (!field.required)
        return FieldValidationStatus.VALID;

    //We are here means, this is a required field. This means, we need to make sure
    //the field (which can potentially have multiple values) has at least one value
    //in at least one language.
    let valueFound = false;
    for (let i = 0; !valueFound && field?.values && (i < field.values?.length); ++i) {
        valueFound = field.values[i]?.value?.trim().length > 0;
    }

    //Validation is successful as long as some value is in an inner field.
    return valueFound ? FieldValidationStatus.VALID : FieldValidationStatus.VALUE_REQUIRED;
}

export function validateOptionsField(field: OptionsField): FieldValidationStatus {
    //If the field itself is not a required field, no need to select a value, so the field is always valid
    if (!field.required)
        return FieldValidationStatus.VALID;

    let selectionFound = false;
    for (let i = 0; !selectionFound && field.options?.$values && i < field.options?.$values?.length; ++i) {
        selectionFound = field.options?.$values[i].selected;
    }

    return selectionFound ? FieldValidationStatus.VALID : FieldValidationStatus.VALUE_REQUIRED;
}

