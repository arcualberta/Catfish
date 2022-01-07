import { MutationTree } from 'vuex';
import { State } from './state';
import { Guid } from 'guid-typescript'
import { FieldContainer } from '../../shared/models/fieldContainer'

//Declare MutationTypes
export enum Mutations {
    SET_IDS = 'SET_IDS',
    SET_FORM = 'SET_FORM',
    SET_SUBMISSIONS = 'SET_SUBMISSIONS'
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
    },
    [Mutations.SET_SUBMISSIONS](state: State, payload: FieldContainer[]) {
        state.formInstances = payload
    }
}
