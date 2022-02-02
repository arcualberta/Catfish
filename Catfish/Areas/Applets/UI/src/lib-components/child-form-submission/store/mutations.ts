import { MutationTree } from 'vuex';
import { Guid } from 'guid-typescript';

import { State } from './state';
import { FieldContainer } from '../../shared/models/fieldContainer';
import { mutations as formSubmissionMutations } from '../../form-submission/store/mutations';
import { TypedArray } from '../../shared/store/form-submission-utils';

//Declare MutationTypes
export enum Mutations  {
    SET_SUBMISSIONS = 'SET_SUBMISSIONS',
    SET_PATENT_ITEM_ID = 'SET_PATENT_ITEM_ID',
    APPEND_CHILD_INSTANCE ='APPEND_CHILD_INSTANCE'
}

//Create a mutation tree that implement all mutation interfaces
export const mutations: MutationTree<State> = {
    [Mutations.SET_SUBMISSIONS](state: State, payload: TypedArray<FieldContainer>) {
        state.formInstances = payload
    },
    [Mutations.SET_PATENT_ITEM_ID](state: State, payload: Guid) {
        state.itemInstanceId = payload
    },
    [Mutations.APPEND_CHILD_INSTANCE](state: State, payload: FieldContainer) {
        state.formInstances?.$values.unshift(payload);
    },
    ...formSubmissionMutations
}
