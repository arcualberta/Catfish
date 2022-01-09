import { eFieldType, Field, FieldContainer, MonolingualTextField, MultilingualTextField, OptionsField, Option } from '../models/fieldContainer'
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

export function flattenFieldInputs(container: FieldContainer, state: FlattenedFormFiledState) {

    //Populating the flattenedTextModels and flattenedOptionModels arrays
    container.fields.forEach((value: Field) => {

        //Try to parse the field type into eFieldType
        const absTypeStr = value.$type?.substring(0, value.$type.indexOf(","));
        const fieldTypeStr = absTypeStr.substring(absTypeStr.lastIndexOf(".") + 1);
        const fieldType: eFieldType = eFieldType[fieldTypeStr as keyof typeof eFieldType];

        const isMonoLinqualField = fieldType === eFieldType.DateField || fieldType === eFieldType.DecimalField || fieldType === eFieldType.EmailField || fieldType === eFieldType.IntegerField || fieldType === eFieldType.MonolingualTextField;
        const isMultilingualField = fieldType === eFieldType.TextArea || fieldType === eFieldType.TextField;
        const isOptionsField = fieldType === eFieldType.CheckboxField || fieldType === eFieldType.RadioField || fieldType === eFieldType.SelectField;

        if (isMonoLinqualField) {
            //Iterating through each text value and adding them to the flattened dictionary
            (value as MonolingualTextField).values?.forEach((txtVal: Text) => {
                state.flattenedTextModels[txtVal.id.toString()] = txtVal;
            })
        }
        else if (isMultilingualField) {
            //Iterating through each value as a multilingual field
            (value as MultilingualTextField).values?.forEach((multilingualVal: TextCollection) => {
                //Iterating through each text value and adding them to the flattened dictionary
                multilingualVal.values.forEach((txtVal: Text) => {
                    state.flattenedTextModels[txtVal.id.toString()] = txtVal;
                })
            })
        }
        else if (isOptionsField) {
            //Itenrating through each option and adding them to the flattened options dictionary
            (value as OptionsField).options.forEach((opt: Option) => {
                state.flattenedOptionModels[opt.id.toString()] = opt;
            })
        }
    })

    //console.log("flattenedTextModels\n", JSON.stringify(state.flattenedTextModels))
    //console.log("flattenedOptionModels\n", JSON.stringify(state.flattenedOptionModels))

}