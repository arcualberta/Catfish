import { ActionTree } from 'vuex';
import { State } from './state';
import { Mutations } from './mutations';

//Declare ActionTypes
export enum Actions {
  FILTER_BY_KEYWORDS = 'FILTER_BY_KEYWORDS',
}

export const actions: ActionTree<State, any> = {

  async [Actions.FILTER_BY_KEYWORDS](store, keywords: string[]) {
    const concatenatedKeywords = keywords.join('|')
    const api = `https://localhost:44385/api/tilegrid/?keywords=${concatenatedKeywords}`;

    const res = await fetch(api);
    const data = await res.json()
    store.commit(Mutations.SET_TILES, data);
  }

}