import { Guid } from 'guid-typescript'

import { FieldContainer } from '../../shared/models/fieldContainer'
import { State as FormSubmissionStateInterface, state as formSubmissionState } from '../../form-submission/store/state'
import { TypedArray } from '../../shared/store/form-submission-utils'

//Declare State interface
export interface State extends FormSubmissionStateInterface {
    itemInstanceId: Guid | null;
    childResponseFormId: Guid | null;
    childResponseForm: FieldContainer | null;
    formInstances: TypedArray<FieldContainer> | null;
}

export const state: State = {
    itemInstanceId: null,
    childResponseFormId: null,
    childResponseForm: null,
    formInstances: null,
    ...formSubmissionState
}
