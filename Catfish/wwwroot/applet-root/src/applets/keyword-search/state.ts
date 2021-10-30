import { Guid } from 'guid-typescript'
//import { SearchOutput } from '../../models'
//import { KeywordQueryModel } from '../../models/keywords'

//Declare State interface
export interface State {
  keywordQueryModel: KeywordQueryModel | null,
  searchResult: SearchOutput | null,
  offset: number,
  max: number,
  pageId: Guid | null,
  blockId: Guid | null
}

export const state: State = {
  keywordQueryModel: null,
  searchResult: null,
  offset: 0,
  max: 25,
  pageId: null,
  blockId: null
}
