import { MutationTree } from 'vuex';
import { State } from './state';
import { Guid } from 'guid-typescript'
import { eValidationStatus, FieldContainer } from '../../shared/models/fieldContainer';
import { flattenFieldInputs, FlattenedFormFiledMutations } from '../../shared/store/form-submission-utils'
import { validateFields } from '../../shared/store/form-validators';

export enum SubmissionStatus {
    None = "None",
    InProgress = "InProgress",
    Success = "Success",
    Fail = "Fail"
}
//Declare MutationTypes
export enum Mutations {
    SET_IDS = 'SET_IDS',
    SET_FORM = 'SET_FORM',
    SET_SUBMISSIONS = 'SET_SUBMISSIONS',
    SET_SUBMISSION_STATUS='SET_SUBMISSION_STATUS'
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
        //console.log("state.form\n", JSON.stringify(state.form))

        flattenFieldInputs(state.form, state)
    },
    [Mutations.SET_SUBMISSIONS](state: State, payload: FieldContainer[]) {
        state.formInstances = payload
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
        state.submissionStatus = SubmissionStatus[status as keyof typeof SubmissionStatus];
    },
}
