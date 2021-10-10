import { ActionTree } from 'vuex';
import { State } from './state';
import { Mutations } from './mutations';
import { SearchParams } from '../../models'
import { KeywordSource } from '../../models/keywords'

//Declare ActionTypes
export enum Actions {
  LOAD_KEYWORDS = 'LOAD_KEYWORDS',
  FILTER_BY_KEYWORDS = 'FILTER_BY_KEYWORDS',
  NEXT_PAGE = 'NEXT_PAGE',
  PREVIOUS_PAGE = 'PREVIOUS_PAGE'
}

export const actions: ActionTree<State, any> = {

  async [Actions.LOAD_KEYWORDS](store, params: KeywordSource) {

    const api = window.location.origin +
      `/api/tilegrid/keywords/page/${params.pageId}/block/${params.blockId}`;
    console.log('Keyword Load API: ', api)

    const res = await fetch(api);
    const data = await res.json()
    store.commit(Mutations.SET_KEYWORDS, data);
  },

  async [Actions.FILTER_BY_KEYWORDS](store, params: SearchParams) {

    const api = window.location.origin +
      `/api/tilegrid/?pageId=${params?.pageId}&blockId=${params?.blockId}&keywords=${params?.keywords?.join('|')}&offset=${params?.offset}&max=${params?.max}`;

    console.log("Item Load API: ", api)

    const res = await fetch(api);
    const data = await res.json()
    store.commit(Mutations.SET_TILES, data);
  }
}

