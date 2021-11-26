import { Guid } from 'guid-typescript'

//Declare State interface
export interface State {
  
  Id: Guid | null;
  
}

export const state: State = {
  
  Id: null
  
}
