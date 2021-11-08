import { ActionTree } from 'vuex';
import { State } from './state';

//Declare ActionTypes
export enum Actions {
  INIT_FILTER = 'INIT_FILTER',
  FILTER_BY_KEYWORDS = 'FILTER_BY_KEYWORDS',
  NEXT_PAGE = 'NEXT_PAGE',
  PREVIOUS_PAGE = 'PREVIOUS_PAGE'
}

export const actions: ActionTree<State, any> = {

}

