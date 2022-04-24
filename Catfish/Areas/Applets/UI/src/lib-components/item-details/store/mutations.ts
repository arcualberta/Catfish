import { MutationTree } from 'vuex';
import { State, UserPermission } from './state';
import { Guid } from 'guid-typescript'
import { Item } from '../../shared/models/item';
import { flattenFieldInputs } from '../../shared/store/form-submission-utils';
import { mutations as formSubmissionMutations } from '../../shared/store/flattened-form-field-mutations'

//Declare MutationTypes
export enum Mutations {
    SET_ID = 'SET_ID',
    SET_USER_PERMISSIONS ='SET_USER_PERMISSIONS',
    SET_ITEM = 'SET_ITEM'
}

//Create a mutation tree that implement all mutation interfaces
export const mutations: MutationTree<State> = {

    ...formSubmissionMutations,

    [Mutations.SET_ID](state: State, payload: Guid) {
        state.id = payload;
    },
    [Mutations.SET_USER_PERMISSIONS](state: State, payload: UserPermission[]) {
        state.permissionList = payload;
    },
    [Mutations.SET_ITEM](state: State, payload: Item) {
        state.item = payload;

        //Iterating through each entry in the data container and the metadata sets 
        //and adding them to the flattened field lists
        payload?.dataContainer?.$values?.forEach(fieldContainer => { flattenFieldInputs(fieldContainer, state); });
        payload?.metadataSets?.$values?.forEach(fieldContainer => { flattenFieldInputs(fieldContainer, state); });
    },
}