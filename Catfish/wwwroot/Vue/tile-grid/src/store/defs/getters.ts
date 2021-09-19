import { GetterTree } from 'vuex';
import { State, Tile } from './state';

export const getters: GetterTree<State, State> = {
  items: (state) : Tile[] => {
    return state.items
  },
}
