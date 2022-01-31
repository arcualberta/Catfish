import { MutationTree } from 'vuex';
import { Guid } from 'guid-typescript';

import { State } from './state';
import { FieldContainer } from '../../shared/models/fieldContainer';
import { mutations as formSubmissionMutations } from '../../form-submission/store/mutations';

//Declare MutationTypes
export enum Mutations  {
    SET_SUBMISSIONS = 'SET_SUBMISSIONS',
    SET_PATENT_ITEM_ID = 'SET_PATENT_ITEM_ID'
}

//Create a mutation tree that implement all mutation interfaces
export const mutations: MutationTree<State> = {
    [Mutations.SET_SUBMISSIONS](state: State, payload: FieldContainer[]) {
        state.formInstances = payload
    },
    [Mutations.SET_PATENT_ITEM_ID](state: State, payload: Guid) {
        state.itemInstanceId = payload
    },
    ...formSubmissionMutations
}
