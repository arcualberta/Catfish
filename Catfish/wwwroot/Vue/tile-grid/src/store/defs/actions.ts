import { ActionTree } from 'vuex';
import { State } from './state';
import { Mutations } from './mutations';

//Declare ActionTypes
export enum Actions {
  FILTER_BY_KEYWORDS = 'FILTER_BY_KEYWORDS',
}

export const actions: ActionTree<State, any> = {

  async [Actions.FILTER_BY_KEYWORDS](store, keywords: string | string[]) {

    if (typeof keywords !== "string")
      keywords = keywords.join('|');

    localStorage.selectedKeywords = keywords;

    const api = window.location.origin + `/api/tilegrid/?keywords=${keywords}`;

    const res = await fetch(api);
    const data = await res.json()
    store.commit(Mutations.SET_TILES, data);
  }

}