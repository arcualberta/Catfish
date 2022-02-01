import { Guid } from 'guid-typescript'

import { FieldContainer } from '../../shared/models/fieldContainer'
import { State as FormSubmissionStateInterface, state as formSubmissionStateObject } from '../../form-submission/store/state'

//Declare State interface
export interface State extends FormSubmissionStateInterface {
    itemInstanceId: Guid | null;
    formInstances: FieldContainer[];
}

export const state: State = {
    itemInstanceId: null,
    formInstances: [],
    ...formSubmissionStateObject
}
