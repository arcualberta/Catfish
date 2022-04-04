import { ActionTree } from 'vuex';
import { State } from './state';
import { Mutations } from './mutations';
import { KeywordSource } from '../models/keywords'
import { SearchParams } from '../models';

//Declare ActionTypes
export enum Actions {
  INIT_FILTER = 'INIT_FILTER',
  FILTER_BY_KEYWORDS = 'FILTER_BY_KEYWORDS',
  NEXT_PAGE = 'NEXT_PAGE',
  PREVIOUS_PAGE = 'PREVIOUS_PAGE',
  FRESH_SEARCH = 'FRESH_SEARCH',
  SAVE_KEYWORDS = 'SAVE_KEYWORDS',
  SEARCH_FREE_TEXT = 'SEARCH_FREE_TEXT',
  SET_SEARCH_TEXT='SET_SEARCH_TEXT'
}

export const actions: ActionTree<State, any> = {

 [Actions.INIT_FILTER](store) {

    //console.log('Store: ', JSON.stringify(store.state))

    const api = window.location.origin +
      `/applets/api/keywordsearch/keywords/page/${store.state.pageId}/block/${store.state.blockId}`;
    console.log('Keyword Load API: ', api)

    fetch(api)
      .then(response => response.json())
      .then(data => {
          store.commit(Mutations.SET_KEYWORDS, data)
        
      });
  },

  [Actions.FILTER_BY_KEYWORDS](store) {
    console.log("Dispatched Actions.FILTER_BY_KEYWORDS. Query model: ", JSON.stringify(store.state.keywordQueryModel))

    //Saving current search parameters in the local storage
    if (store.state.blockId) {
      const searchParams = {
        keywords: store.state.keywordQueryModel,
        offset: store.state.offset,
        max: store.state.max
        } as SearchParams;

        
        localStorage.setItem(store.getters.searchParamStorageKey, JSON.stringify(searchParams));
    }
      
    const api = window.location.origin + `/applets/api/keywordsearch/items/`;
    console.log("Item Load API: ", api)

    const formData = new FormData();
    if (store.state.pageId)
      formData.append("pageId", store.state.pageId.toString());
    if (store.state.blockId)
      formData.append("blockId", store.state.blockId.toString());
    
    formData.append("offset", store.state.offset.toString());
    formData.append("max", store.state.max.toString());
    formData.append("queryParams", JSON.stringify(store.state.keywordQueryModel));
    
    //console.log("Form Data: ", formData)

    fetch(api, {
      method: 'POST', // or 'PUT'
      body: formData
    })
      .then(response => response.json())
      .then(data => {
        store.commit(Mutations.SET_RESULTS, data);
      })
      .catch((error) => {
        console.error('Item Load API Error:', error);
      });
  },

  [Actions.NEXT_PAGE](store) {
    store.commit(Mutations.SET_OFFSET, store.state.offset + store.state.max);
    store.dispatch(Actions.FILTER_BY_KEYWORDS);
  },

  [Actions.PREVIOUS_PAGE](store) {
    const offset = Math.max(store.state.offset - store.state.max, 0);
    store.commit(Mutations.SET_OFFSET, offset);
    store.dispatch(Actions.FILTER_BY_KEYWORDS);
  },

  [Actions.FRESH_SEARCH](store, pageSize: number) {
    store.commit(Mutations.SET_OFFSET, 0);
    if (pageSize)
      store.commit(Mutations.SET_PAGE_SIZE, pageSize);
    store.dispatch(Actions.FILTER_BY_KEYWORDS);
    },

    [Actions.SAVE_KEYWORDS](store, source: KeywordSource) {
       // console.log("save keywords action :" + JSON.stringify(source));
        store.commit(Mutations.SET_KEYWORDS, source);
  },

  ////async [Actions.INIT_FILTER_ASYNC](store, source: KeywordSource) {

  ////  store.commit(Mutations.SET_SOURCE, source);

  ////  const api = window.location.origin +
  ////    `/applets/api/keywordsearch/keywords/page/${source.pageId}/block/${source.blockId}`;
  ////  console.log('Keyword Load API: ', api)

  ////  const res = await fetch(api);
  ////  const data = await res.json()
  ////  store.commit(Mutations.SET_KEYWORDS, data);
  ////},
  [Actions.SET_SEARCH_TEXT](store, text: string) {

     // console.log("set serch text: " + text);
        store.commit(Mutations.SET_FREE_TEXT_SEARCH, text);
  },
[Actions.SEARCH_FREE_TEXT](store) {
    console.log("executing search for: " + store.state.freeSearchText?.toString());
    const api = window.location.origin +
        `/api/solr/executefreetext/${store.state.freeSearchText?.toString()}`
  
      fetch(api)
     .then(response => response.json())
     .then(data => {
       console.log("results:")     
       console.log(data)

      });
    }

}

