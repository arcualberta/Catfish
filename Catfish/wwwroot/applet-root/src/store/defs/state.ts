import { Guid } from 'guid-typescript'

//Declare State interface
export interface State {
  appletName: string | null,
  pageId: Guid | null,
  blockId: Guid | null
}

export const state: State = {
  appletName: null,
  pageId: null,
  blockId: null
}
