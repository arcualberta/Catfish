import { MutationTree } from 'vuex';
import { State } from './state';
import { Guid } from 'guid-typescript'
import { eFieldType, Field, FieldContainer, MonolingualTextField, MultilingualTextField, OptionsField, Option } from '../../shared/models/fieldContainer'
import { TextCollection, Text } from '../../shared/models/textModels';

//Declare MutationTypes
export enum Mutations {
    SET_IDS = 'SET_IDS',
    SET_FORM = 'SET_FORM',
    SET_SUBMISSIONS = 'SET_SUBMISSIONS',
    SET_TEXT_VALUE = 'SET_TEXT_VALUE',
    SET_OPTION_VALUE = 'SET_OPTION_VALUE'
}

//Create a mutation tree that implement all mutation interfaces
export const mutations: MutationTree<State> = {

    [Mutations.SET_IDS](state: State, payload: Guid[]) {
        state.itemInstanceId = payload[0];
        state.itemTemplateId = payload[1];
        state.formId = payload[2];
    },
    [Mutations.SET_FORM](state: State, payload: FieldContainer) {
        state.form = payload;
        //console.log("form\n", JSON.stringify(state.form))

        //Populating the flattenedTextModels and flattenedOptionModels arrays
        payload.fields.forEach((value: Field) => {

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

    },
    [Mutations.SET_SUBMISSIONS](state: State, payload: FieldContainer[]) {
        state.formInstances = payload
    },
    [Mutations.SET_TEXT_VALUE](state: State, payload: { id: Guid; val: string }) {
        state.flattenedTextModels[payload.id.toString()].value = payload.val;
    },
    [Mutations.SET_OPTION_VALUE](state: State, payload: { id: Guid; isSelected: boolean }) {
        state.flattenedOptionModels[payload.id.toString()].selected = payload.isSelected;
    }
}
