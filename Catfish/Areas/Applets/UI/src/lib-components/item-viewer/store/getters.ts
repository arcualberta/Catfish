import { GetterTree } from 'vuex';
import { State } from './state';

export const getters: GetterTree<State, State> = {
  rootDataItem: state => {
    return state.item?.dataContainer.filter(dc => dc.isRoot)[0];
  }
}
