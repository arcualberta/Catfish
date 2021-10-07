import { SearchOutput } from '../../models'

//Declare State interface
export interface State {
  searchResult: SearchOutput | null
}


export const state: State = {
  searchResult: null,
}
