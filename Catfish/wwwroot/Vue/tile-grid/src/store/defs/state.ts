import { SearchOutput } from '../../models'
import { KeywordQueryModel } from '../../models/keywords'

//Declare State interface
export interface State {
  keywordQueryModel: KeywordQueryModel | null,
  searchResult: SearchOutput | null
}

export const state: State = {
  keywordQueryModel: null,
  searchResult: null,
}
