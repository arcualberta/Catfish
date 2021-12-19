import { MutationTree } from 'vuex';
import { IndexingStatus } from '../models';
import { State } from './state';

//Declare MutationTypes
export enum Mutations {
    SET_REINDEX_PAGE_STATUS = 'SET_REINDEX_PAGE_STATUS',
    SET_REINDEX_DATA_STATUS = 'SET_REINDEX_DATA_STATUS',
    SET_REINDEX_STATUS = 'SET_REINDEX_STATUS'
}

//Create a mutation tree that implement all mutation interfaces
export const mutations: MutationTree<State> = {

    [Mutations.SET_REINDEX_PAGE_STATUS](state: State, payload: boolean) {
        console.log("SET_REINDEX_PAGE_STATUS: ", payload);
        state.indexingStatus.pageIndexingInprogress = payload;
    },

    [Mutations.SET_REINDEX_DATA_STATUS](state: State, payload: boolean) {
        console.log("SET_REINDEX_DATA_STATUS: ", payload);
        state.indexingStatus.dataIndexingInprogress = payload;
    },

    [Mutations.SET_REINDEX_STATUS](state: State, payload: IndexingStatus) {
        console.log("SET_REINDEX_STATUS: ", payload);
        state.indexingStatus = payload;
    }

}
