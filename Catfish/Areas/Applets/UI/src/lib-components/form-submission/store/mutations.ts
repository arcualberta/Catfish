import { MutationTree } from 'vuex';
import { State } from './state';
import { Guid } from 'guid-typescript'
import { eSubmissionStatus } from '../../shared/store/form-submission-utils'
import { mutations as formSubmissionMutations } from '../../shared/store/flattened-form-field-mutations'
import { FieldContainer } from '../../shared/models/fieldContainer';

//Declare MutationTypes
export enum Mutations {
    SET_ITEM_TEMPLATE_ID = 'SET_ITEM_TEMPLATE_ID',
    SET_FORM_ID = 'SET_FORM_ID',
    SET_SUBMISSION_STATUS = 'SET_SUBMISSION_STATUS',
    SET_COLLECTION_ID = 'SET_COLLECTION_ID',
    SET_GROUP_ID = 'SET_GROUP_ID',
    SET_FORM = 'SET_FORM',
}

//Create a mutation tree that implement all mutation interfaces
export const mutations: MutationTree<State> = {
    ...formSubmissionMutations,
    [Mutations.SET_ITEM_TEMPLATE_ID](state: State, payload: Guid) {
        state.itemTemplateId = payload;
    },
    [Mutations.SET_FORM_ID](state: State, payload: Guid) {
        state.formId = payload;
    },
    [Mutations.SET_COLLECTION_ID](state: State, payload: Guid) {
        state.collectionId = payload;
    },
    [Mutations.SET_GROUP_ID](state: State, payload: Guid) {
        state.groupId = payload;
    },
    [Mutations.SET_SUBMISSION_STATUS](state: State, status: string) {
        //const fieldType: eFieldType = eFieldType[fieldTypeStr as keyof typeof eFieldType];
        state.submissionStatus = eSubmissionStatus[status as keyof typeof eSubmissionStatus];
    },
    [Mutations.SET_FORM](state: State, payload: FieldContainer) {
        state.form = payload;
    },
}
