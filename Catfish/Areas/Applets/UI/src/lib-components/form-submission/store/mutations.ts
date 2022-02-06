﻿import { MutationTree } from 'vuex';
import { State } from './state';
import { Guid } from 'guid-typescript'
import { eValidationStatus, FieldContainer } from '../../shared/models/fieldContainer';
import { flattenFieldInputs, FlattenedFormFiledMutations, eSubmissionStatus } from '../../shared/store/form-submission-utils'
import { validateFields } from '../../shared/store/form-validators';

//export enum SubmissionStatus {
//    None = "None",
//    InProgress = "InProgress",
//    Success = "Success",
//    Fail = "Fail"
//}
//Declare MutationTypes
export enum Mutations {
    CLEAR_FLATTENED_FIELD_MODELS = 'CLEAR_FLATTENED_FIELD_mODELS',
    SET_ITEM_TEMPLATE_ID = 'SET_ITEM_TEMPLATE_ID',
    SET_FORM_ID = 'SET_FORM_ID',
    SET_FORM = 'SET_FORM',
    SET_SUBMISSION_STATUS = 'SET_SUBMISSION_STATUS',
    SET_COLLECTION_ID = 'SET_COLLECTION_ID',
    SET_GROUP_ID='SET_GROUP_ID'
}

//Create a mutation tree that implement all mutation interfaces
export const mutations: MutationTree<State> = {

    [Mutations.CLEAR_FLATTENED_FIELD_MODELS](state: State) {
        state.flattenedTextModels = {};
        state.flattenedOptionModels = {};
    },
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
    [Mutations.SET_FORM](state: State, payload: FieldContainer) {
        state.form = payload
        flattenFieldInputs(state.form, state)
        //console.log("state\n", JSON.stringify(state))
    },
    [FlattenedFormFiledMutations.SET_TEXT_VALUE](state: State, payload: { id: Guid; val: string }) {
        //console.log("payload id:", payload.id, "   payload value: ", payload.val)
        state.flattenedTextModels[payload.id.toString()].value = payload.val;
        //console.log("state flattenedTextModels", JSON.stringify(state.flattenedTextModels))

        //Re-validating the form
        if (state.form?.validationStatus === eValidationStatus.INVALID)
            validateFields(state.form);

    },
    [FlattenedFormFiledMutations.SET_OPTION_VALUE](state: State, payload: { id: Guid; isSelected: boolean }) {
        state.flattenedOptionModels[payload.id.toString()].selected = payload.isSelected;
    },
    [Mutations.SET_SUBMISSION_STATUS](state: State, status: string) {

        //const fieldType: eFieldType = eFieldType[fieldTypeStr as keyof typeof eFieldType];
        state.submissionStatus = eSubmissionStatus[status as keyof typeof eSubmissionStatus];
    },
}
