import { MutationTree } from 'vuex';
import { State } from './state';
import { SearchResult } from '../../models'

//Declare MutationTypes
export enum Mutations {
  SET_TILES = 'SET_TILES',
}

//Create a mutation tree that implement all mutation interfaces
export const mutations: MutationTree<State> = {
  [Mutations.SET_TILES](state: State, payload: SearchResult) {
    console.log('Payload: ', payload)
    state.searchResult = payload;
  },
}
