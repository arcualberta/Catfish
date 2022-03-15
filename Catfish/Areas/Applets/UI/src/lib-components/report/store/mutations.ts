import { Guid } from "guid-typescript";
import { MutationTree } from "vuex";
import { Item } from "../../item-viewer/models/item";
import { State, ReportCell, ReportRow, ReportField } from "./state";




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
    [Mutations.SET_REPORT_FIELDS](state: State, payload: ReportField[]) {
        state.reportFields = payload
    },
    [Mutations.SET_REPORT_DATA](state: State, payload: Item[]) {
        state.reportData = [] as ReportRow[];
        for (let i = 0; i < payload.length; ++i) {
            const item = payload[i];
            const reportRow = {} as ReportRow
            state.reportFields?.forEach(repField => {
                const form = item.dataContainer.$values.filter(frm => frm.id === repField.formTemplateId)[0];
                const field = form?.fields.$values.filter(fld => fld.id === repField.fieldId)[0];
                const cell = { formId: repField.formTemplateId, fieldId: repField.fieldId, value: field.id.toString() } as ReportCell;
                reportRow.cells?.push(cell)
            })
            state.reportData?.push(reportRow);
		}
    },
}