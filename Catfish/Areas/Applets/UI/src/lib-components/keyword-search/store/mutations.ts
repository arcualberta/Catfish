import { MutationTree } from 'vuex';
import { State } from './state';
import { SearchOutput } from '../models'
import { KeywordQueryModel, KeywordSource } from '../models/keywords';

//Declare MutationTypes
export enum Mutations {
  SET_SOURCE = 'SET_SOURCE',
  SET_KEYWORDS = 'SET_KEYWORDS',
  SET_RESULTS = 'SET_RESULTS',
  SET_OFFSET = 'SET_OFFSET'
  
}

//Create a mutation tree that implement all mutation interfaces
export const mutations: MutationTree<State> = {

  [Mutations.SET_SOURCE](state: State, payload: KeywordSource) {
    state.pageId = payload.pageId;
    state.blockId = payload.blockId;
  },

  [Mutations.SET_KEYWORDS](state: State, payload: KeywordQueryModel) {
    console.log('SET_KEYWORDS Payload: ', payload)
    state.keywordQueryModel = payload;
  },

  [Mutations.SET_RESULTS](state: State, payload: SearchOutput) {
    console.log('SET_RESULTS Payload: ', JSON.stringify(payload))
    console.log('SET_RESULTS Payload.first: ', payload.first)
    state.searchResult = payload;
    state.offset = payload.first - 1;
  },

  [Mutations.SET_OFFSET](state: State, payload: number) {
    console.log('SET_OFFSET Payload: ', JSON.stringify(payload))
    state.offset = payload;
  }

}
