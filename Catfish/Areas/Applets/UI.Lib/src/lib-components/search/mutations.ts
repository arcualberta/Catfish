import { MutationTree } from 'vuex';

import { mutations as baseMutations } from '../shared/store/mutations'
import { State } from './state';
import { KeywordQueryModel, KeywordIndex, SearchOutput } from './models';
//import { mutations as itemViewerMutations } from '../../item-viewer/store/mutations';

//Declare MutationTypes
export enum Mutations {
    SET_SOURCE = 'SET_SOURCE',
    SET_DATA_SERVICE_API_ROOT = 'SET_DATA_SERVICE_API_ROOT',
    SET_PAGE_SERVICE_API_ROOT = 'SET_PAGE_SERVICE_API_ROOT',
    SET_SOLR_SERVICE_API_ROOT = 'SET_SOLR_SERVICE_API_ROOT',

    SET_KEYWORDS = 'SET_KEYWORDS',
    SET_FREE_TEXT = 'SET_FREE_TEXT',
    SET_RESULTS = 'SET_RESULTS',
    SET_OFFSET = 'SET_OFFSET',
    SET_PAGE_SIZE = 'SET_PAGE_SIZE',
    TOGGLE_KEYWORD = 'TOGGLE_KEYWORD',
    CLEAR_KEYWORD_SELECTIONS = 'CLEAR_KEYWORD_SELECTIONS',
    SELECT_KEYWORD = 'SELECT_KEYWORD',
    CLEAR_KEYWORD = 'CLEAR_KEYWORD',
}

////const Mutations = { ...BaseMutations, ...ExtendedMutations }
////export type Mutations = BaseMutations | ExtendedMutations;

//Create a mutation tree that implement all mutation interfaces
export const mutations: MutationTree<State> = {
    //...itemViewerMutations,
    ...baseMutations,

    [Mutations.SET_KEYWORDS](state: State, payload: KeywordQueryModel) {
        console.log('SET_KEYWORDS Payload: ', payload)
        state.keywordQueryModel = payload;
    },

    [Mutations.SET_FREE_TEXT](state: State, payload: string) {
        // console.log('mutation set text: payload: ', payload)
        state.freeSearchText = payload;
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
    [Mutations.TOGGLE_KEYWORD](state: State, payload: KeywordIndex) {
        if (state.keywordQueryModel)
            state.keywordQueryModel.containers[payload.containerIndex].fields[payload.fieldIndex].selected[payload.valueIndex] = !state.keywordQueryModel.containers[payload.containerIndex].fields[payload.fieldIndex].selected[payload.valueIndex];
    },
    [Mutations.SELECT_KEYWORD](state: State, payload: KeywordIndex) {
        if (state.keywordQueryModel)
            state.keywordQueryModel.containers[payload.containerIndex].fields[payload.fieldIndex].selected[payload.valueIndex] = true;
    },
    [Mutations.CLEAR_KEYWORD](state: State, payload: KeywordIndex) {
        if (state.keywordQueryModel)
            state.keywordQueryModel.containers[payload.containerIndex].fields[payload.fieldIndex].selected[payload.valueIndex] = false;
    },
    [Mutations.CLEAR_KEYWORD_SELECTIONS](state: State) {
        state.keywordQueryModel?.containers.forEach(cont => cont.fields.forEach(field => field.selected = new Array(field.values.length).fill(false)))
    },
}
