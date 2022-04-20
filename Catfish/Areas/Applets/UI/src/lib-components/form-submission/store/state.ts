import { Guid } from 'guid-typescript'
import { FieldContainer } from '../../shared/models/fieldContainer'
import { eSubmissionStatus } from '../../shared/store/form-submission-utils'
import { FlattenedFormFiledState } from '../../shared/store/flattened-form-field-state'


//Declare State interface
export interface State extends FlattenedFormFiledState {
    itemTemplateId: Guid | null;
    formId: Guid | null;
    collectionId: Guid | null;
    groupId: Guid | null;
    submissionStatus: eSubmissionStatus;
    formLoadAPI: string | null;
    formSubmissionAPI: string | null;
    form: FieldContainer | null;
}

export const state: State = {
    itemTemplateId: null,
    formId: null,
    collectionId: null,
    groupId: null,
    form: null,
    flattenedTextModels: {},
    flattenedOptionModels: {},
    flattenedFileModels: {},
    submissionStatus: eSubmissionStatus.None,
    formLoadAPI: null,
    formSubmissionAPI: null
}
