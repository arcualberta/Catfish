import { Guid } from "guid-typescript";
import { MutationTree } from "vuex";
import { State } from "./state";




export enum Mutations {
    SET_TEMPLATE_ID = "SET_TEMPLATE_ID",
    SET_COLLECTION_ID = "SET_COLLECTION_ID",
    SET_GROUP_ID = "SET_GROUP_ID",
    SET_REPORT_FIELDS = "SET_REPORT_FIELDS",
    SET_REPORT_DATA = "SET_REPORT_DATA"
}


export const mutations: MutationTree<State> = {

    [Mutations.SET_TEMPLATE_ID](state: State, payload: Guid) {
        state.itemTemplateID = payload
    },
    [Mutations.SET_COLLECTION_ID](state: State, payload: Guid) {
        state.collectionID = payload
    },
    [Mutations.SET_GROUP_ID](state: State, payload: Guid) {
        state.groupId = payload
    },
}