import { ActionTree } from 'vuex';
//import { State } from './state';
import { Mutations } from './mutations';
import { SearchParams, KeywordSource } from './models';
//import { actions as itemViewerActions} from '../../item-viewer/store/actions';

//Declare ActionTypes
export enum Actions {
    INIT_FILTER = 'INIT_FILTER',
    FILTER_BY_KEYWORDS = 'FILTER_BY_KEYWORDS',
    NEXT_PAGE = 'NEXT_PAGE',
    PREVIOUS_PAGE = 'PREVIOUS_PAGE',
    FRESH_SEARCH = 'FRESH_SEARCH',
    SAVE_KEYWORDS = 'SAVE_KEYWORDS',
    SEARCH_FREE_TEXT = 'SEARCH_FREE_TEXT',
    SET_SEARCH_TEXT = 'SET_SEARCH_TEXT',
    LOAD_ITEM = 'LOAD_ITEM',
}


export const actions: ActionTree<any, any> = {
    //...itemViewerActions,

    [Actions.INIT_FILTER](store) {

        const common = store.rootState.common;
        //console.log('Store: ', JSON.stringify(store.state))
        const apiRoot = common?.dataServiceApiRoot ? common.dataServiceApiRoot : window.location.origin + '/applets/api';
        const api = apiRoot + `/keywordsearch/keywords/page/${common.pageId}/block/${common.blockId}`;
        console.log('Keyword Load API: ', api)

        fetch(api)
            .then(response => response.json())
            .then(data => {
                store.commit(Mutations.SET_KEYWORDS, data)

            });
    },

    [Actions.FILTER_BY_KEYWORDS](store) {
        //Saving current search parameters in the local storage
        if (store.state.blockId) {
            const searchParams = {
                keywords: store.state.keywordQueryModel,
                offset: store.state.offset,
                max: store.state.max
            } as SearchParams;


            localStorage.setItem(store.getters.searchParamStorageKey, JSON.stringify(searchParams));
        }

        const common = store.rootState.common;

        const apiRoot = common.dataServiceApiRoot ? common.dataServiceApiRoot : window.location.origin + '/applets/api';
        const api = apiRoot + '/keywordsearch';//`/keywordsearch/items/`;
         console.log("Item Load API: ", api)

        const formData = new FormData();

        //MR: Mey 10 2022 -- commented out pageId and block Id, added template, collection and group Ids

        //if (common.pageId)
        //    formData.append("pageId", common.pageId.toString());
        //if (common.blockId)
        //    formData.append("blockId", common.blockId.toString());

        if (common.templateId)
            formData.append("templateId", common.templateId.toString())
        if (common.collectionId)
            formData.append("collectionId", common.collectionId.toString())
        if (common.groupId)
            formData.append("groupId", common.groupId.toString())

        formData.append("offset", store.state.offset.toString());
        formData.append("max", store.state.max.toString());
        formData.append("queryParams", JSON.stringify(store.state.keywordQueryModel));

        //MR April 27 2022, add freetextsearch
        let freeText = store.state.freeSearchText ? store.state.freeSearchText : "";
        formData.append("searchText", freeText);


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
    [Actions.SET_SEARCH_TEXT](store, text: string) {

        // console.log("set serch text: " + text);
        store.commit(Mutations.SET_FREE_TEXT, text);
    }
}
