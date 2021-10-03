import { ActionTree } from 'vuex';
import { State } from './state';
import { Mutations } from './mutations';

//Declare ActionTypes
export enum Actions {
  FILTER_BY_KEYWORDS = 'FILTER_BY_KEYWORDS',
  NEXT_PAGE = 'NEXT_PAGE',
  PREVIOUS_PAGE = 'PREVIOUS_PAGE'
}

export const actions: ActionTree<State, any> = {

  async [Actions.FILTER_BY_KEYWORDS](store, keywords: string | string[]) {

    //const keywords = (typeof params.keywords === "string") ? params.keywords : params.keywords.join('|');
    if (typeof keywords !== "string")
      keywords = keywords.join('|');

    localStorage.selectedKeywords = keywords;
    const offset = 0;
    const max = 25;

    const api = window.location.origin + `/api/tilegrid/?keywords=${keywords}&offset=${offset}&max=${max}`;

    const res = await fetch(api);
    const data = await res.json()
    store.commit(Mutations.SET_TILES, data);
  }
}

export interface SearchParams {
  keywords: string | string[],
  offset: number,
  max: number
}