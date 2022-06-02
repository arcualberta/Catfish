import * as models from './models'
import { eFieldType, eDataElementType } from './enumerations'

export const getFieldName = (obj: models.Field | models.FieldContainer): string => {
    return obj?.name?.values?.$values
        .map(txt => txt.value)
        .join(" | ") as string;
}

export const getSelectedFieldLabels = (options: models.Option[]): string => {
    return options?.filter(opt => opt.selected)
        .map(opt => opt.optionText?.values.$values
            .map(txt => txt.value)
            .join(" / ")
        )
        .join(", ")
}

export const getTypeString = (obj: models.Field | models.Text | models.Option | models.FileReference): string => {
    const typeName: string = obj?.$type.substring(0, obj.$type.indexOf(","));
    return typeName?.substring(typeName.lastIndexOf(".") + 1);
}

export const getFieldType = (field: models.Field): eFieldType => {
    const typeName: string = getTypeString(field);
    return (<any>eFieldType)[typeName];
}

export const testFieldType = (field: models.Field, type: eFieldType): boolean => {
    return getFieldType(field) === type;
}

export const getDataElementType = (obj: models.DataElementType): eDataElementType => {
    const typeName: string = getTypeString(obj);
    return (<any>eDataElementType)[typeName];
}

export const validateEmail = (email: string): boolean => {
    if (email) {

        return true;
    }
    return false;
}


export const validateForm = (form: models.FieldContainer): boolean => {
    form.validationStatus = true;
    form.fields.$values.forEach(field => {
        switch (getFieldType(field)) {
            case eFieldType.AttachmentField:
                form.validationStatus &&= validateAttachmentField(field as models.AttachmentField);
                break;
            case eFieldType.AudioRecorderField:
                form.validationStatus &&= validateAttachmentField(field as models.AttachmentField);
                break;
            case eFieldType.CheckboxField:
            case eFieldType.RadioField:
            case eFieldType.SelectField:
                form.validationStatus &&= validateOptionsField(field as models.OptionsField);
                break;
            case eFieldType.CompositeField:
                form.validationStatus &&= validateCompositeField(field as models.Field);
                break;
            case eFieldType.DateField:
                form.validationStatus &&= validateDateField(field as models.MonolingualTextField);
                break;
            case eFieldType.DecimalField:
            case eFieldType.IntegerField:
                form.validationStatus &&= validateNumberField(field as models.MonolingualTextField);
                break;
            case eFieldType.EmailField:
                form.validationStatus &&= validateEmailField(field as models.MonolingualTextField);
                break;
            case eFieldType.FieldContainerReference:
                form.validationStatus &&= validateFieldContainerReferenceField(field as models.FieldContainerReference);
                break;
            case eFieldType.InfoSection:
                //NOTHING TO VALIDATE
                break;
            case eFieldType.MonolingualTextField:
                form.validationStatus &&= validateMonolingualTextField(field as models.MonolingualTextField);
                break;
            case eFieldType.TableField:
                form.validationStatus &&= validateTableField(field as models.Field);
                break;
            case eFieldType.TextArea:
            case eFieldType.TextField:
                form.validationStatus &&= validateMultilingualTextField(field as models.MultilingualTextField);
                break;
            default:
                field.validationError = "No validation method available."
                field.validationStatus = false;
                form.validationStatus &&= false;
                break;
        }
    })

    return form.validationStatus;
   
}

export const validateMultilingualTextField = (field: models.MultilingualTextField): boolean => {

    if (field.required) {

    }

    return true;
}

export const validateMonolingualTextField = (field: models.MonolingualTextField): boolean => {
     
    if (field.required) {
        
        const txtVals = (field as models.MonolingualTextField)?.values?.$values.filter(txt => txt.value?.length > 0)
        if (txtVals && txtVals?.length > 0)
            return true;
        else
            return false
    }

    return true;
}

export const validateEmailField = (field: models.MonolingualTextField): boolean => {

    let valid = false as boolean;
    const txtVals = (field as models.MonolingualTextField)?.values?.$values.filter(txt => txt.value?.length > 0);
    const regularExpression = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if (field.required) {
        if (txtVals && regularExpression.test(String(txtVals[0]).toLowerCase()))
            valid = true;
    } else {
        if (txtVals && txtVals.length > 0) {
            if (regularExpression.test(String(txtVals[0]).toLowerCase()))
                valid = true;
        } else {
            valid = true;
        }
    }

    return valid;
}

export const validateNumberField = (field: models.MonolingualTextField): boolean => {

    if (field) {

    }

    return false;
}

export const validateAttachmentField = (field: models.AttachmentField): boolean => {

    if (field) {

    }

    return false;
}


export const validateOptionsField = (field: models.OptionsField): boolean => {

    if (field) {

    }

    return false;
}

export const validateCompositeField = (field: models.Field): boolean => {

    if (field) {

        return true;
    }

    return false;
}


export const validateDateField = (field: models.MonolingualTextField): boolean => {

    if (field) {

    }

    return false;
}

export const validateFieldContainerReferenceField = (field: models.FieldContainerReference): boolean => {

    if (field) {
        true;
    }

    return false;
}

export const validateTableField = (field: models.Field): boolean => {

    if (field) {
        true;
    }

    return false;
}