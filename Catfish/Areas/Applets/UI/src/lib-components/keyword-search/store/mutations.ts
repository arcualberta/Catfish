import { MutationTree } from 'vuex';
import { State } from './state';
import { SearchOutput } from '../models'
import { KeywordQueryModel, KeywordSource } from '../models/keywords';
import { DataAttribute, QueryParameter } from '../../shared/props'

//Declare MutationTypes
export enum Mutations {
    SET_SOURCE = 'SET_SOURCE',
    SET_KEYWORDS = 'SET_KEYWORDS',
    SET_RESULTS = 'SET_RESULTS',
    SET_OFFSET = 'SET_OFFSET',
    SET_PAGE_SIZE = 'SET_PAGE_SIZE',
    SET_FREE_TEXT_SEARCH = 'SET_FREE_TEXT_SEARCH',
    SET_INIT_PARAMS = 'SET_INIT_PARAMS',
}

//Create a mutation tree that implement all mutation interfaces
export const mutations: MutationTree<State> = {

    [Mutations.SET_SOURCE](state: State, payload: KeywordSource) {
        state.pageId = payload.pageId;
        state.blockId = payload.blockId;
    },

    [Mutations.SET_KEYWORDS](state: State, payload: KeywordQueryModel) {
        console.log('SET_KEYWORDS Payload: ', payload)
        state.keywordQueryModel = payload;
    },

    [Mutations.SET_RESULTS](state: State, payload: SearchOutput) {
        state.searchResult = payload;
        state.offset = payload.first - 1;
    },

    [Mutations.SET_OFFSET](state: State, payload: number) {
        //console.log('SET_OFFSET: payload: ', payload)
        state.offset = payload;
    },

    [Mutations.SET_PAGE_SIZE](state: State, payload: number) {
        //console.log('SET_PAGE_SIZE: payload: ', payload)
        state.max = payload;
    },
    [Mutations.SET_FREE_TEXT_SEARCH](state: State, payload: string) {
        // console.log('mutation set text: payload: ', payload)
        state.freeSearchText = payload;
    },
    [Mutations.SET_INIT_PARAMS](state: State, payload: { dataAttributes: DataAttribute | null, queryParams: QueryParameter | null}) {
        // console.log('mutation set text: payload: ', payload)
        state.dataAttributes = payload.dataAttributes;
        state.keywordQueryModel = payload.queryParams;
    }

}
