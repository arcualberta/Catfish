import { MutationTree } from 'vuex';
import { State } from './state';
import { Tile } from './tile';

//Declare MutationTypes
export enum MutationTypes {
  SET_TILES = 'SET_TILES',
}

//Declare all mutation-function interfaces available
export type Mutations<S = State> = {
  [MutationTypes.SET_TILES](state: S, payload: Tile[]): void
}

//Create a mutation tree that implement all mutation interfaces
export const mutations: MutationTree<State> & Mutations = {
  [MutationTypes.SET_TILES](state: State, payload: Tile[]) {
    state.tiles = payload
  },
}
