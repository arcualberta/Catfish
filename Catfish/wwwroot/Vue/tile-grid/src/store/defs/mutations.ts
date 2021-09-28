import { MutationTree } from 'vuex';
import { State, Item } from './state';

//Declare MutationTypes
export enum Mutations {
  SET_TILES = 'SET_TILES',
}

//Create a mutation tree that implement all mutation interfaces
export const mutations: MutationTree<State> = {
  [Mutations.SET_TILES](state: State, payload: Item[]) {
    state.items = payload
    console.log('Payload: ', payload)
  },
}
