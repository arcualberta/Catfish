import { MutationTree } from 'vuex';
import { State } from './state';
import { Block, DataSource, Page } from '../models/cmsModels'

//Declare MutationTypes
export enum Mutations {
    SET_SOURCE = 'SET_SOURCE',
    SET_MODEL = 'SET_MODEL'
}

//Create a mutation tree that implement all mutation interfaces
export const mutations: MutationTree<State> = {

    [Mutations.SET_SOURCE](state: State, payload: DataSource) {
        state.pageId = payload.pageId;
        state.blockId = payload.blockId;
    },

    [Mutations.SET_MODEL](state: State, payload: Block | Page) {
        state.model = payload;
    }
}
