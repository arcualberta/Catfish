import { MutationTree } from 'vuex';
import { State } from './state';
import { Guid } from 'guid-typescript'
import { FieldContainer } from '../../shared/models/fieldContainer';
import { flattenFieldInputs } from '../../shared/store/form-submission-utils'

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
        state.form = payload
        //console.log("form\n", JSON.stringify(state.form))

        flattenFieldInputs(state.form, state)
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
