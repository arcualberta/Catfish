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
                field.validationStatus = validateAttachmentField(field as models.AttachmentField);
                form.validationStatus = field.validationStatus;
                break;
            case eFieldType.AudioRecorderField:
                form.validationStatus &&= validateAttachmentField(field as models.AttachmentField);
                break;
            case eFieldType.CheckboxField:
            case eFieldType.RadioField:
            case eFieldType.SelectField:
               
                field.validationStatus = validateOptionsField(field as models.OptionsField);
                form.validationStatus = field.validationStatus;

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
                field.validationStatus = validateEmailField(field as models.MonolingualTextField);
                form.validationStatus = field.validationStatus;
                break;
            case eFieldType.FieldContainerReference:
                form.validationStatus &&= validateFieldContainerReferenceField(field as models.FieldContainerReference);
                break;
            case eFieldType.InfoSection:
                //NOTHING TO VALIDATE
                break;
            case eFieldType.MonolingualTextField:
               
                 field.validationStatus = validateMonolingualTextField(field as models.MonolingualTextField);
                form.validationStatus = field.validationStatus
                break;
            case eFieldType.TableField:
                form.validationStatus &&= validateTableField(field as models.Field);
                break;
            case eFieldType.TextArea:
            case eFieldType.TextField:
                field.validationStatus = validateMultilingualTextField(field as models.MultilingualTextField);
                form.validationStatus = field.validationStatus;
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
    field.validationStatus = true;
    let valueFound = false;
    for (let i = 0; !valueFound && field?.values && (i < field.values?.$values?.length); ++i) {
        const txtCollection = field?.values?.$values[i];
        for (let k = 0; !valueFound && txtCollection.values && (k < txtCollection.values?.$values.length); ++k) {
            valueFound = txtCollection.values?.$values[k]?.value?.trim().length > 0;
        }
    }


    if (field.required) {
       
        let valueFound = false;
        for (let i = 0; !valueFound && field?.values && (i < field.values?.$values?.length); ++i) {
            const txtCollection = field?.values?.$values[i];
            for (let k = 0; !valueFound && txtCollection.values && (k < txtCollection.values?.$values.length); ++k) {
                valueFound = txtCollection.values?.$values[k]?.value?.trim().length > 0; 
            }
        }

        if (!valueFound) {
            field.validationStatus = false;
            field.validationError = "This field is required";
        }
    }
    return field.validationStatus;
}

export const validateMonolingualTextField = (field: models.MonolingualTextField): boolean => {
    field.validationStatus = true;
    if (field.required) {
        const txtVals = (field as models.MonolingualTextField)?.values?.$values.filter(txt => txt.value?.length > 0)
        if (!(txtVals && txtVals?.length > 0)) {
            field.validationStatus = false;
            field.validationError = "This field is required";
        }
    }

    return field.validationStatus;
}

export const validateEmailField = (field: models.MonolingualTextField): boolean => {

    field.validationStatus = validateMonolingualTextField(field); //false as boolean;

    if (field.validationStatus) {
        const txtVals = (field as models.MonolingualTextField)?.values?.$values.filter(txt => txt.value?.length > 0);
        const regularExpression = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

        txtVals?.forEach(txtVal => {
            if (!regularExpression.test(String(txtVal.value).toLowerCase())) {
                field.validationStatus &&= false;
                field.validationError = "Invalid email address"
            }
        });
    }

    return field.validationStatus;
}

export const validateNumberField = (field: models.MonolingualTextField): boolean => {
    field.validationStatus = validateMonolingualTextField(field);
    if (field.validationStatus) {
        const txtVals = (field as models.MonolingualTextField)?.values?.$values.filter(txt => txt.value?.length > 0);
        txtVals?.forEach(txtVal => {
            if (!parseInt(txtVal.value)) {
                field.validationStatus &&= false;
                field.validationError = "Require a number value"
            }
        });
       
    }
    return field.validationStatus;
}

export const validateAttachmentField = (field: models.AttachmentField): boolean => {
    field.validationStatus = true;
    if (field.required) {
        if (!((field as models.AttachmentField)?.files?.$values.length > 0)) {
            field.validationStatus = false;
            field.validationError = "This field is required, please attach a file"
        }
           
    }

    return field.validationStatus;
}


export const validateOptionsField = (field: models.OptionsField): boolean => {
    field.validationStatus = true;
    if (field.required) {
       
        const selectedVals = (field as models.OptionsField).options?.$values.filter(val => val.selected == true);
        if (! (selectedVals.length > 0)) {
            field.validationStatus = false;
            field.validationError="Please select at least one"
        }
           
    }
    return field.validationStatus;
}

export const validateCompositeField = (field: models.Field): boolean => {

    field.validationStatus = true;

    return field.validationStatus;
}


export const validateDateField = (field: models.MonolingualTextField): boolean => {

    field.validationStatus = true;

    return field.validationStatus;
}

export const validateFieldContainerReferenceField = (field: models.FieldContainerReference): boolean => {
    field.validationStatus = true;
   
    return field.validationStatus;
}

export const validateTableField = (field: models.Field): boolean => {

    field.validationStatus = true;

    return field.validationStatus;
}