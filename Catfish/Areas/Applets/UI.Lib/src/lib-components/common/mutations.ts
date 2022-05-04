import { MutationTree } from 'vuex';
import { State } from './state';

//Declare MutationTypes
export enum Mutations {
    SET_SOURCE = 'SET_SOURCE',
    SET_DATA_SERVICE_API_ROOT = 'SET_DATA_SERVICE_API_ROOT',
    SET_PAGE_SERVICE_API_ROOT = 'SET_PAGE_SERVICE_API_ROOT',
    SET_SOLR_SERVICE_API_ROOT = 'SET_SOLR_SERVICE_API_ROOT',
}

//Create a mutation tree that implement all mutation interfaces
export const mutations: MutationTree<State> = {

    [Mutations.SET_DATA_SERVICE_API_ROOT](state: State, apiRoot: string) {
        state.dataServiceApiRoot = trimTrailingSlash(apiRoot);
    },

    [Mutations.SET_PAGE_SERVICE_API_ROOT](state: State, apiRoot: string) {
        state.pageServiceApiRoot = trimTrailingSlash(apiRoot);
    },

    [Mutations.SET_SOLR_SERVICE_API_ROOT](state: State, apiRoot: string) {
        state.solrServiceApiRoot = trimTrailingSlash(apiRoot);
    },
}

function trimTrailingSlash(url: string) {
    return url?.endsWith('/') ? url?.substring(0, url.length - 1) : url;
}
