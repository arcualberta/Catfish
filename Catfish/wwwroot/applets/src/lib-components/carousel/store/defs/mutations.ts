import { MutationTree } from 'vuex';
import { State } from './state';

//Declare MutationTypes
export enum Mutations {
  SET_SOURCE = 'SET_SOURCE',
  SET_KEYWORDS = 'SET_KEYWORDS',
  SET_TILES = 'SET_TILES'
}

//Create a mutation tree that implement all mutation interfaces
export const mutations: MutationTree<State> = {


}
