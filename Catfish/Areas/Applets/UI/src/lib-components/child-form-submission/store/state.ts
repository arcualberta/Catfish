import { FieldContainer } from '../../shared/models/fieldContainer'
import { SubmissionStatus } from '../../form-submission/store/mutations'
import {State as FormSubmissionState } from '../../form-submission/store/state'

//Declare State interface
export interface State extends FormSubmissionState {
    formInstances: FieldContainer[];
}

export const state: State = {

    itemInstanceId: null,
    itemTemplateId: null,
    formId: null,
    form: null,
    flattenedTextModels: {},
    flattenedOptionModels: {},
    submissionStatus: SubmissionStatus.None,
    formInstances: [],
}
