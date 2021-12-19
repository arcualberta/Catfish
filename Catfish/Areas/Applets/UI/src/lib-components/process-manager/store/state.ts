import { Guid } from 'guid-typescript'

//Declare State interface
export interface State {
	id: Guid | null;
}

export const state: State = {
  id: null
}
