import { eFieldType, FieldContainer, eValidationStatus, MonolingualTextField, MultilingualTextField, OptionsField } from '../models/fieldContainer'
import { FieldContainerUtils } from './form-submission-utils'

export abstract class RegExpressions {
    public static Email = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
    public static Number = /^\d+$/;
    public static Decimal = /^[+-]?(\d+\.?\d*|\.\d+)$/;

}

export function validateMultilingualTextField(field: MultilingualTextField): eValidationStatus {
    //If the field itself is not a required field, any contents in inner fields 
    //will be valid, including none
    if (!field.required)
        return eValidationStatus.VALID;

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
    return valueFound ? eValidationStatus.VALID : eValidationStatus.VALUE_REQUIRED;
}

export function validateMonolingualTextField(field: MonolingualTextField, validationRegExp: RegExp | null): eValidationStatus {

    //Go through each value in the monolingual field. Set valueFound to true if at least one value is found.
    //If validationRegExp is specified, then validate each non-empty value against the validationRegExp.
    //If any reg-exp validations failed, then set the final validation result to validation error.
    let valueFound = false;
    let validationStatus = eValidationStatus.VALID;
    for (let i = 0; field?.values && (i < field.values?.$values.length); ++i) {
        const valStr = field.values.$values[i]?.value?.trim();
        if (valStr?.length > 0) {
            valueFound = true;

            if (validationRegExp && !validationRegExp.test(valStr))
                validationStatus = eValidationStatus.INVALID;
		}
    }

    if (field.required && !valueFound)
        validationStatus = eValidationStatus.VALUE_REQUIRED;

    //Validation is successful as long as some value is in an inner field.
    return validationStatus;
}

export function validateMonolingualNumberField(field: MonolingualTextField): eValidationStatus {

    //Go through each value in the monolingual field. Set valueFound to true if at least one value is found.
   
    let valueFound = false;
    let validationStatus = eValidationStatus.VALID;
    for (let i = 0; field?.values && (i < field.values?.$values.length); ++i) {
        const valStr = field.values.$values[i]?.value;
       //if it's empty or null the typeof will not return string 'number'
        if (typeof (valStr) === 'number') {
            valueFound = true;
           // console.log("type: number")
        }
    }

    if (field.required && !valueFound)
        validationStatus = eValidationStatus.VALUE_REQUIRED;

    //Validation is successful as long as some value is in an inner field.
    return validationStatus;
}

export function validateOptionsField(field: OptionsField): eValidationStatus {
    //If the field itself is not a required field, no need to select a value, so the field is always valid
    if (!field.required)
        return eValidationStatus.VALID;

    let selectionFound = false;
    for (let i = 0; !selectionFound && field.options?.$values && i < field.options?.$values?.length; ++i) {
        selectionFound = field.options?.$values[i].selected;
    }

    return selectionFound ? eValidationStatus.VALID : eValidationStatus.VALUE_REQUIRED;
}

export function validateFields(form: FieldContainer): boolean {
    let valid = true;
    form.fields?.$values?.forEach(field => {
        switch (FieldContainerUtils.getFieldType(field)) {
            case eFieldType.AttachmentField:
                break;
            case eFieldType.CheckboxField:
            case eFieldType.RadioField:
            case eFieldType.SelectField:
                field.validationStatus = validateOptionsField(field as OptionsField);
                break;
            case eFieldType.CompositeField:
                break;
            case eFieldType.DateField:
                field.validationStatus = validateMonolingualTextField(field as MonolingualTextField, null);
                break;
            case eFieldType.DecimalField:
            case eFieldType.IntegerField:
                field.validationStatus = validateMonolingualNumberField(field as MonolingualTextField);
                break;
            case eFieldType.EmailField:
                field.validationStatus = validateMonolingualTextField(field as MonolingualTextField, RegExpressions.Email);
                break;
            case eFieldType.FieldContainerReference:
                break;
            //case eFieldType.IntegerField:
            //    field.validationStatus = validateMonolingualNumberField(field as MonolingualTextField);
            //    break;
            case eFieldType.MonolingualTextField:
                field.validationStatus = validateMonolingualTextField(field as MonolingualTextField, null);
                break;
            //case eFieldType.RadioField:
            //    field.validationStatus = validateOptionsField(field as OptionsField);
            //    break;
            //case eFieldType.SelectField:
            //    field.validationStatus = validateOptionsField(field as OptionsField);
            //    break;
            case eFieldType.TableField:
                break;
            case eFieldType.TextArea:
                field.validationStatus = validateMultilingualTextField(field as MultilingualTextField);
                break;
            case eFieldType.TextField:
                field.validationStatus = validateMultilingualTextField(field as MultilingualTextField);
                break;
            case eFieldType.AudioRecorderField:
                field.validationStatus = eValidationStatus.VALID;
                break;
        }

        valid = valid && (field.validationStatus === eValidationStatus.VALID);
    });

    form.validationStatus = valid ? null : eValidationStatus.INVALID
    return valid;
}