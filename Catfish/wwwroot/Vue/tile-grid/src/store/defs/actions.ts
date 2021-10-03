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

  async [Actions.FILTER_BY_KEYWORDS](store, params: SearchParams | undefined) {

    //If search params is not specified, try to load it from the Local Storage. If it is not null,
    //save it into the Local Storage
    if (params === undefined) {
      params = (localStorage.keywordSearchParams)
        ? JSON.parse(localStorage.keywordSearchParams)
        : { keywords: [], offset: 0, max: 0 };
    }

    localStorage.searchParams = JSON.stringify(params);

    const api = window.location.origin +
      `/api/tilegrid/?keywords=${params?.keywords?.join('|')}&offset=${params?.offset}&max=${params?.max}`;

    console.log("Item Load API: ", api)

    const res = await fetch(api);
    const data = await res.json()
    store.commit(Mutations.SET_TILES, data);
  }
}

