import { MutationTree } from 'vuex';
import { State } from './state';
import { Guid } from 'guid-typescript';

//Declare MutationTypes
export enum Mutations {
    SET_PAGE_ID = 'SET_PAGE_ID',
    SET_BLOCK_ID = 'SET_BLOCK_ID',
    SET_DATA_SERVICE_API_ROOT = 'SET_DATA_SERVICE_API_ROOT',
    SET_PAGE_SERVICE_API_ROOT = 'SET_PAGE_SERVICE_API_ROOT',
    SET_SOLR_SERVICE_API_ROOT = 'SET_SOLR_SERVICE_API_ROOT',
}

//Create a mutation tree that implement all mutation interfaces
export const mutations: MutationTree<State> = {

    [Mutations.SET_PAGE_ID](state: State, id: Guid) {
        state.pageId = id;
        console.log('SET_PAGE_ID: ', id)
    },

    [Mutations.SET_BLOCK_ID](state: State, id: Guid) {
        state.blockId = id;
        console.log('SET_BLOCK_ID: ', id,)
    },

    [Mutations.SET_DATA_SERVICE_API_ROOT](state: State, apiRoot: string) {
        state.dataServiceApiRoot = trimTrailingSlash(apiRoot);
        console.log('SET_DATA_SERVICE_API_ROOT: ', apiRoot)
    },

    [Mutations.SET_PAGE_SERVICE_API_ROOT](state: State, apiRoot: string) {
        state.pageServiceApiRoot = trimTrailingSlash(apiRoot);
        console.log('SET_PAGE_SERVICE_API_ROOT: ', apiRoot)
    },

    [Mutations.SET_SOLR_SERVICE_API_ROOT](state: State, apiRoot: string) {
        state.solrServiceApiRoot = trimTrailingSlash(apiRoot);
        console.log('SET_SOLR_SERVICE_API_ROOT: ', apiRoot)
    },
}

function trimTrailingSlash(url: string) {
    return url?.endsWith('/') ? url?.substring(0, url.length - 1) : url;
}
