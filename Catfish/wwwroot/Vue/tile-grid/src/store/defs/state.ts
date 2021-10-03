import { SearchResult } from '../../models'

//Declare State interface
export interface State {
  searchResult: SearchResult | null
}


export const state: State = {
  searchResult: null,
}
