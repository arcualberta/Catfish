import { State as ItemStateInterface, state as itemState } from '../../item-viewer/store/state'

//Declare State interface
export interface State extends ItemStateInterface {
}

export const state: State = {
    ...itemState
}
