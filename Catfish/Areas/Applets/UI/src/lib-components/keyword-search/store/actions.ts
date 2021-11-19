import { ActionTree } from 'vuex';
import { State } from './state';
import { Mutations } from './mutations';
//import { SearchParams } from '../../models'
import { KeywordSource } from '../models/keywords'

//Declare ActionTypes
export enum Actions {
  INIT_FILTER = 'INIT_FILTER',
  INIT_FILTER_ASYNC = 'INIT_FILTER_ASYNC',
  FILTER_BY_KEYWORDS = 'FILTER_BY_KEYWORDS',
  NEXT_PAGE = 'NEXT_PAGE',
  PREVIOUS_PAGE = 'PREVIOUS_PAGE'
}

export const actions: ActionTree<State, any> = {

 [Actions.INIT_FILTER](store, source: KeywordSource) {

    console.log('Store: ', store)
    console.log('Source: ', source)

    store.commit(Mutations.SET_SOURCE, source);

    const api = window.location.origin +
      `/applets/api/keywordsearch/keywords/page/${source.pageId}/block/${source.blockId}`;
    console.log('Keyword Load API: ', api)

    fetch(api)
      .then(response => response.json())
      .then(data => {
        console.log("Fetch results: ", data);
        store.commit(Mutations.SET_KEYWORDS, data)
      });

  },

  [Actions.FILTER_BY_KEYWORDS](store) {
    console.log("Dispatched Actions.FILTER_BY_KEYWORDS. Query model: ", JSON.stringify(store.state.keywordQueryModel))

    const api = window.location.origin + `/applets/api/keywordsearch/items/`;
    console.log("Item Load API: ", api)

    const formData = new FormData();
    if (store.state.pageId) formData.append("pageId", store.state.pageId.toString());
    if (store.state.blockId) formData.append("blockId", store.state.blockId.toString());

    formData.append("offset", store.state.offset.toString());
    formData.append("max", store.state.max.toString());
    formData.append("queryParams", JSON.stringify(store.state.keywordQueryModel));

    console.log("Form Data: ", formData)

    fetch(api, {
      method: 'POST', // or 'PUT'
      body: formData
    })
      .then(response => response.json())
      .then(data => {
        store.commit(Mutations.SET_RESULTS, data);
      })
      .catch((error) => {
        console.error('Error:', error);
      });
  }

  ////async [Actions.INIT_FILTER_ASYNC](store, source: KeywordSource) {

  ////  store.commit(Mutations.SET_SOURCE, source);

  ////  const api = window.location.origin +
  ////    `/applets/api/keywordsearch/keywords/page/${source.pageId}/block/${source.blockId}`;
  ////  console.log('Keyword Load API: ', api)

  ////  const res = await fetch(api);
  ////  const data = await res.json()
  ////  store.commit(Mutations.SET_KEYWORDS, data);
  ////},



}

