import { MutationTree } from 'vuex';
import { State } from './state';
import { DataSource } from '../models/cmsModels'

//Declare MutationTypes
export enum Mutations {
    SET_SOURCE = 'SET_SOURCE',
    SET_DATA_SERVICE_API_ROOT = 'SET_DATA_SERVICE_API_ROOT',
    SET_PAGE_SERVICE_API_ROOT = 'SET_PAGE_SERVICE_API_ROOT',
    SET_SOLR_SERVICE_API_ROOT = 'SET_SOLR_SERVICE_API_ROOT',
}

//Create a mutation tree that implement all mutation interfaces
export const mutations: MutationTree<State> = {

    [Mutations.SET_SOURCE](state: State, payload: DataSource) {
        state.pageId = payload.pageId;
        state.blockId = payload.blockId;
    },

    [Mutations.SET_DATA_SERVICE_API_ROOT](state: State, apiRoot: string) {
        state.dataServiceApiRoot = apiRoot?.endsWith('/') ? apiRoot?.substring(0, apiRoot.length - 1) : apiRoot;
    },

    [Mutations.SET_PAGE_SERVICE_API_ROOT](state: State, apiRoot: string) {
        state.pagesApiRoot = apiRoot?.endsWith('/') ? apiRoot?.substring(0, apiRoot.length - 1) : apiRoot;
    },

    [Mutations.SET_SOLR_SERVICE_API_ROOT](state: State, apiRoot: string) {
        state.solrApiRoot = apiRoot?.endsWith('/') ? apiRoot?.substring(0, apiRoot.length - 1) : apiRoot;
    },


}
