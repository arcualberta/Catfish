import { ActionTree } from 'vuex';
import { State } from './state';
import { Mutations } from './mutations';
import { SearchParams } from '../../models'

//Declare ActionTypes
export enum Actions {
  FILTER_BY_KEYWORDS = 'FILTER_BY_KEYWORDS',
  NEXT_PAGE = 'NEXT_PAGE',
  PREVIOUS_PAGE = 'PREVIOUS_PAGE'
}

export const actions: ActionTree<State, any> = {

  async [Actions.FILTER_BY_KEYWORDS](store, params: SearchParams) {

    const api = window.location.origin +
      `/api/tilegrid/?collectionId=${params?.collectionId}&keywords=${params?.keywords?.join('|')}&offset=${params?.offset}&max=${params?.max}`;

    console.log("Item Load API: ", api)

    const res = await fetch(api);
    const data = await res.json()
    store.commit(Mutations.SET_TILES, data);
  }
}

