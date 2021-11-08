import { Guid } from 'guid-typescript'

//Declare State interface
export interface State {
  pageId: Guid | null;
  blockId: Guid | null;
}

export const state: State = {
  pageId: null,
  blockId: null
}
