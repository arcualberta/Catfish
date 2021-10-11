import { MutationTree } from 'vuex';
import { State } from './state';
import { SearchOutput } from '../../models'
import { KeywordQueryModel } from '../../models/keywords';

//Declare MutationTypes
export enum Mutations {
  SET_KEYWORDS = 'SET_KEYWORDS',
  SET_TILES = 'SET_TILES'
}

//Create a mutation tree that implement all mutation interfaces
export const mutations: MutationTree<State> = {

  [Mutations.SET_KEYWORDS](state: State, payload: KeywordQueryModel) {
    console.log('SET_KEYWORDS Payload: ', payload)

    ////Updating the "selected" array associated with each field
    //for (const cIdx in payload.containers)
    //  for (const fIdx in payload.containers[cIdx].fields)
    //    payload.containers[cIdx].fields[fIdx].selected = new Array(payload.containers[cIdx].fields[fIdx].values.length).fill(false);

    state.keywordQueryModel = payload;
  },

  [Mutations.SET_TILES](state: State, payload: SearchOutput) {
    console.log('SET_TILES Payload: ', payload)
    state.searchResult = payload;
  }
}
