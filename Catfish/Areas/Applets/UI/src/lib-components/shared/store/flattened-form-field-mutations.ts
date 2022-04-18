import { Guid } from 'guid-typescript'
import { MutationTree } from 'vuex';
import { FlattenedFormFiledState as State } from './flattened-form-field-state';
import { flattenFieldInputs } from './form-submission-utils'
import { FieldContainer, eValidationStatus } from '../../shared/models/fieldContainer';
import { validateFields } from '../../shared/store/form-validators';

export enum FlattenedFormFiledMutations {
    SET_TEXT_VALUE = 'SET_TEXT_VALUE',
    SET_OPTION_VALUE = 'SET_OPTION_VALUE',
    ADD_FILE = 'ADD_FILE',
    REMOVE_FILE = 'REMOVE_FILE',
    CLEAR_FIELD_DATA = 'CLEAR_FIELD_DATA',
    REMOVE_FIELD_CONTAINERS = 'REMOVE_FIELD_CONTAINERS',
    APPEND_FIELD_DATA = 'APPEND_FIELD_DATA',
}

//Create a mutation tree that implement all mutation interfaces
export const mutations: MutationTree<State> = {
    [FlattenedFormFiledMutations.SET_TEXT_VALUE](state: State, payload: { id: Guid; val: string }) {
        //console.log("payload id:", payload.id, "   payload value: ", payload.val)
        state.flattenedTextModels[payload.id.toString()].value = payload.val;
        //console.log("state flattenedTextModels", JSON.stringify(state.flattenedTextModels))

        //Re-validating the forms
        state.fieldContainers?.forEach(fc => {
            if (fc?.validationStatus === eValidationStatus.INVALID)
            validateFields(fc);
        })
    },
    [FlattenedFormFiledMutations.SET_OPTION_VALUE](state: State, payload: { id: Guid; isSelected: boolean }) {
        state.flattenedOptionModels[payload.id.toString()].selected = payload.isSelected;
    },
    [FlattenedFormFiledMutations.ADD_FILE](state: State, payload: { id: Guid; val: File }) {
        if (!state.flattenedFileModels[payload.id.toString()])
            state.flattenedFileModels[payload.id.toString()] = [] as File[];
        state.flattenedFileModels[payload.id.toString()].push(payload.val);
    },
    [FlattenedFormFiledMutations.REMOVE_FILE](state: State, payload: { id: Guid; index: number }) {
        state.flattenedFileModels[payload.id.toString()]?.splice(payload.index, 1);
    },
    [FlattenedFormFiledMutations.CLEAR_FIELD_DATA](state: State) {
        //Iterate through all Text elements in state.flattenedTextModels 
        Object.keys(state.flattenedTextModels).forEach(function (key) {
            state.flattenedTextModels[key].value = '';
        });

        // Iterate through all Option elements in state.flattenedOptionModels
        Object.keys(state.flattenedOptionModels).forEach(function (key) {
            state.flattenedOptionModels[key].selected = false;
        });

        // Iterate through attachment in state.flattenedOptionModels
        Object.keys(state.flattenedFileModels).forEach(function (key) {
            state.flattenedFileModels[key] = [] as File[];
        });
    },
    [FlattenedFormFiledMutations.REMOVE_FIELD_CONTAINERS](state: State) {
        state.flattenedTextModels = {};
        state.flattenedOptionModels = {};
        state.flattenedFileModels = {}
        state.fieldContainers = [];
    },
    [FlattenedFormFiledMutations.APPEND_FIELD_DATA](state: State, payload: FieldContainer) {
        //console.log('SET_FORM payload:\n', JSON.stringify(payload));
        flattenFieldInputs(payload, state)
    },

}
