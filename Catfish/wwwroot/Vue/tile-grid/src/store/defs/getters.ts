import { GetterTree } from 'vuex';
import { State, Item } from './state';

export const getters: GetterTree<State, State> = {
  items: (state): Item[] => {
    return state.items
  },
}
