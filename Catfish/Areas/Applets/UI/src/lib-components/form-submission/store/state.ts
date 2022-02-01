import { Guid } from 'guid-typescript'
import { FieldContainer } from '../../shared/models/fieldContainer'
import { FlattenedFormFiledState, eSubmissionStatus } from '../../shared/store/form-submission-utils'


//Declare State interface
export interface State extends FlattenedFormFiledState {
    itemTemplateId: Guid | null;
    formId: Guid | null;
    form: FieldContainer | null;
    submissionStatus: eSubmissionStatus;
    formLoadAPI: string | null;
    formSubmissionAPI: string | null;
}

export const state: State = {
    itemTemplateId: null,
    formId: null,
    form: null,
    flattenedTextModels: {},
    flattenedOptionModels: {},
    submissionStatus: eSubmissionStatus.None,
    formLoadAPI: null,
    formSubmissionAPI: null
}
