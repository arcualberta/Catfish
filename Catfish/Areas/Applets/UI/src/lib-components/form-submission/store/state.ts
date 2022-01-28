﻿import { Guid } from 'guid-typescript'
import { FieldContainer } from '../../shared/models/fieldContainer'
import { FlattenedFormFiledState } from '../../shared/store/form-submission-utils'
import { SubmissionStatus } from './mutations'


//Declare State interface
export interface State extends FlattenedFormFiledState {
  
    itemInstanceId: Guid | null;
    itemTemplateId: Guid | null;
    formId: Guid | null;
    form: FieldContainer | null;
    submissionStatus: SubmissionStatus;
}

export const state: State = {

    itemInstanceId: null,
    itemTemplateId: null,
    formId: null,
    form: null,
    flattenedTextModels: {},
    flattenedOptionModels: {},
    submissionStatus: SubmissionStatus.None
}