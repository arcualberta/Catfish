﻿import { MutationTree } from 'vuex';
import { Guid } from 'guid-typescript';

import { State } from './state';
import { FieldContainer } from '../../shared/models/fieldContainer';
import { mutations as formSubmissionMutations } from '../../form-submission/store/mutations';
import { flattenFieldInputs, TypedArray } from '../../shared/store/form-submission-utils';

//Declare MutationTypes
export enum Mutations  {
    SET_SUBMISSIONS = 'SET_SUBMISSIONS',
    SET_PARENT_ITEM_ID = 'SET_PARENT_ITEM_ID',
    APPEND_CHILD_INSTANCE = 'APPEND_CHILD_INSTANCE',
    SET_RESPONSE_FORM_ID = 'SET_RESPONSE_FORM_ID',
    SET_RESPONSE_FORM = 'SET_RESPONSE_FORM',
}

//Create a mutation tree that implement all mutation interfaces
export const mutations: MutationTree<State> = {
    [Mutations.SET_SUBMISSIONS](state: State, payload: TypedArray<FieldContainer>) {
        state.formInstances = payload
    },
    [Mutations.SET_PARENT_ITEM_ID](state: State, payload: Guid) {
        state.itemInstanceId = payload
    },
    [Mutations.APPEND_CHILD_INSTANCE](state: State, payload: FieldContainer) {
        state.formInstances?.$values.unshift(payload);
    },
    [Mutations.SET_RESPONSE_FORM_ID](state: State, payload: Guid) {
        state.childResponseFormId = payload;
    },
    [Mutations.SET_RESPONSE_FORM](state: State, payload: FieldContainer) {
        state.childResponseForm = payload
        flattenFieldInputs(state.childResponseForm, state)
    },
    ...formSubmissionMutations
}
