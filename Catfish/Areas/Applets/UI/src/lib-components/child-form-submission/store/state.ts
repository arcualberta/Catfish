import { Guid } from 'guid-typescript'

import { FieldContainer } from '../../shared/models/fieldContainer'
import { State as FormSubmissionStateInterface, state as formSubmissionStateObject } from '../../form-submission/store/state'
import { TypedArray } from '../../shared/store/form-submission-utils'

//Declare State interface
export interface State extends FormSubmissionStateInterface {
    itemInstanceId: Guid | null;
    formInstances: TypedArray<FieldContainer> | null;
}

export const state: State = {
    itemInstanceId: null,
    formInstances: null,
    ...formSubmissionStateObject
}
