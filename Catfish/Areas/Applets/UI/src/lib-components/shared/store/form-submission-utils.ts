import { eFieldType, Field, FieldContainer, MonolingualTextField, MultilingualTextField, OptionsField, Option, AttachmentField } from '../models/fieldContainer'
import { TextCollection, Text } from '../models/textModels';

//Declare State interface
export interface FlattenedFormFiledState {
    flattenedTextModels: { [key: string]: Text };
    flattenedOptionModels: { [key: string]: Option };
}

export enum FlattenedFormFiledMutations {
    SET_TEXT_VALUE = 'SET_TEXT_VALUE',
    SET_OPTION_VALUE = 'SET_OPTION_VALUE'
}

export abstract class FieldContainerUtils {
    public static cssClass(field: Field): string {
        return field.cssClass + " " + field.fieldCssClass;
    }
    public static getFieldType(field: Field): eFieldType {
        let typeName: string = field?.$type.substring(0, field.$type.indexOf(","));
        typeName = typeName?.substring(typeName.lastIndexOf(".") + 1);
        return eFieldType[typeName as keyof typeof eFieldType];
    }

    public static isAttachmentField(field: Field): boolean {
        return this.getFieldType(field) === eFieldType.AttachmentField
    }
    public static isOptionsField(field: Field): boolean {
        const fieldType = this.getFieldType(field);
        return fieldType === eFieldType.CheckboxField
            || fieldType === eFieldType.RadioField
            || fieldType === eFieldType.SelectField
    }
    public static isCompositeField(field: Field): boolean {
        return this.getFieldType(field) === eFieldType.CompositeField
    }
    public static isDateField(field: Field): boolean {
        return this.getFieldType(field) === eFieldType.DateField
    }
    public static isDecimalField(field: Field): boolean {
        return this.getFieldType(field) === eFieldType.DecimalField
    }
    public static isEmailField(field: Field): boolean {
        return this.getFieldType(field) === eFieldType.EmailField
    }
    public static isFieldContainerReference(field: Field): boolean {
        return this.getFieldType(field) === eFieldType.FieldContainerReference
    }
    public static isInfoSection(field: Field): boolean {
        return this.getFieldType(field) === eFieldType.InfoSection
    }
    public static isIntegerField(field: Field): boolean {
        return this.getFieldType(field) === eFieldType.IntegerField
    }
    public static isMonolingualTextField(field: Field): boolean {
        return this.getFieldType(field) === eFieldType.MonolingualTextField
    }
    public static isTableField(field: Field): boolean {
        return this.getFieldType(field) === eFieldType.TableField
    }
    public static isTextArea(field: Field): boolean {
        return this.getFieldType(field) === eFieldType.TextArea
    }
    public static isTextField(field: Field): boolean {
        return this.getFieldType(field) === eFieldType.TextField
    }
    public static isAudioRecorderField(field: Field): boolean {
        return this.getFieldType(field) === eFieldType.AudioRecorderField
    }
}

export function flattenFieldInputs(container: FieldContainer, state: FlattenedFormFiledState) {

    //Populating the flattenedTextModels and flattenedOptionModels arrays
    container?.fields?.$values?.forEach((value: Field) => {

        //Try to parse the field type into eFieldType
        const absTypeStr = value.$type?.substring(0, value.$type.indexOf(","));
        const fieldTypeStr = absTypeStr.substring(absTypeStr.lastIndexOf(".") + 1);
        const fieldType: eFieldType = eFieldType[fieldTypeStr as keyof typeof eFieldType];

        const isMonoLinqualField = fieldType === eFieldType.DateField || fieldType === eFieldType.DecimalField || fieldType === eFieldType.EmailField || fieldType === eFieldType.IntegerField || fieldType === eFieldType.MonolingualTextField;
        const isMultilingualField = fieldType === eFieldType.TextArea || fieldType === eFieldType.TextField;
        const isOptionsField = fieldType === eFieldType.CheckboxField || fieldType === eFieldType.RadioField || fieldType === eFieldType.SelectField;

        if (isMonoLinqualField) {
            //Iterating through each text value and adding them to the flattened dictionary
            (value as MonolingualTextField).values?.$values?.forEach((txtVal: Text) => {
                state.flattenedTextModels[txtVal.id.toString()] = txtVal;
            })
        }
        else if (isMultilingualField) {
            //Iterating through each value as a multilingual field
            (value as MultilingualTextField).values?.$values?.forEach((multilingualVal: TextCollection) => {
                //Iterating through each text value and adding them to the flattened dictionary
                multilingualVal.values?.$values?.forEach((txtVal: Text) => {
                    state.flattenedTextModels[txtVal.id.toString()] = txtVal;
                })
            })
        }
        else if (isOptionsField) {
            //Itenrating through each option and adding them to the flattened options dictionary
            (value as OptionsField).options?.$values?.forEach((opt: Option) => {
                state.flattenedOptionModels[opt.id.toString()] = opt;
			})
        }
    })

    //console.log("flattenedTextModels\n", JSON.stringify(state.flattenedTextModels))
    //console.log("flattenedOptionModels\n", JSON.stringify(state.flattenedOptionModels))

}

export function clearForm(state: FlattenedFormFiledState) {
    //Iterate through all Text elements in state.flattenedTextModels 
    Object.keys(state.flattenedTextModels).forEach(function (key) {
        state.flattenedTextModels[key].value = '';
    });

    // Iterate through all Option elements in state.flattenedOptionModels
    Object.keys(state.flattenedOptionModels).forEach(function (key) {
        state.flattenedOptionModels[key].selected = false;
    });
}

export function isRequiredMultilingualField(field: MultilingualTextField) {
    return field?.required ? field.required : false;
}

export function isRequiredField(field: Field) {
    return field?.required ? field.required : false;
}

export function isRichTextField(field: MultilingualTextField) {
    return field?.richText ? field.richText : false;
}

export function allowFileExtension(field: AttachmentField) {
    return field.allowedExtensions.toString();
}
export function isAllowMultiple(field: AttachmentField) {
    return field.allowMultipleValues;
}
