import { Guid } from "guid-typescript";
import { MutationTree } from "vuex";
import { State, Report, ReportField, SystemStatus } from "./state";




export enum Mutations {
    SET_TEMPLATE_ID = 'SET_TEMPLATE_ID',
    SET_COLLECTION_ID = 'SET_COLLECTION_ID',
    SET_GROUP_ID = 'SET_GROUP_ID',
    SET_REPORT_FIELDS = 'SET_REPORT_FIELDS',
    SET_REPORT_DATA = 'SET_REPORT_DATA',
    SET_DETAILED_VIEW_URL = 'SET_DETAILED_VIEW_URL',
    SET_STATUS = 'SET_STATUS',
    SET_ID = 'SET_ID',
    SET_OFFSET = 'SET_OFFSET',
    SET_PAGE_SISE = 'SET_PAGE_SISE',
    SET_SITE_URL = 'SET_SITE_URL'
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
    [Mutations.SET_REPORT_FIELDS](state: State, payload: ReportField[]) {
        state.reportFields = payload
    },
    [Mutations.SET_REPORT_DATA](state: State, payload: Report) {
        state.reportData = payload
    },
    [Mutations.SET_DETAILED_VIEW_URL](state: State, payload: string) {
        state.detailedViewUrl = payload
    },
    [Mutations.SET_STATUS](state: State, payload: SystemStatus[]) {
        state.templateStatus = payload
    },
    [Mutations.SET_ID](state: State, payload: Guid) {
        state.id = payload;
    },
    [Mutations.SET_OFFSET](state: State, payload: number) {
        state.offset = payload;
    },
    [Mutations.SET_PAGE_SISE](state: State, payload: number) {
        state.pageSize = payload;
    },
    [Mutations.SET_SITE_URL](state: State, payload: string) {
        state.siteUrl = payload;
    },
}