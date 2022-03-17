import { MutationTree } from 'vuex';
import { State } from './state';
import { Guid } from 'guid-typescript'
import { Item } from "../models/item"

//Declare MutationTypes
export enum Mutations {
    SET_ID = 'SET_ID',
    SET_ITEM = 'SET_ITEM',
    CHANGE_STATE = 'CHANGE_STATE'
}

//Create a mutation tree that implement all mutation interfaces
export const mutations: MutationTree<State> = {

    [Mutations.SET_ID](state: State, payload: Guid) {
        state.id = payload;
    },
    [Mutations.SET_ITEM](state: State, payload: Item) {
        state.item = payload
    },
    [Mutations.CHANGE_STATE](state: State, payload: { itemId: Guid, state: string }) {
        //state.item
        console.log(JSON.stringify(state));
        console.log(JSON.stringify(payload));
    }

}
