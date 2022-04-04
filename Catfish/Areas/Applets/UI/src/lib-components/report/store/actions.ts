import { ActionTree } from 'vuex';
import { State, SearchParams } from './state';
import { Mutations } from './mutations';

export enum Actions {
    LOAD_DATA = 'LOAD_DATA'
}
export const actions: ActionTree<State, any> = {
    [Actions.LOAD_DATA](store, searchParams: SearchParams) {

        console.log('Store: ', JSON.stringify(store.state))

        const api = window.location.origin +
            `/applets/api/items/GetReportData/${store.state.groupId}/template/${store.state.itemTemplateID}/collection/${store.state.collectionID}?startDate=${searchParams.startDate ? searchParams.startDate : ""}&endDate=${searchParams.endDate ? searchParams.endDate : ""}&status=${searchParams.status ? searchParams.status : ""}`;
        console.log('reports Load API: ', api)
        const formData = new FormData();

        //Setting the serialized JSON form model to the datamodel variable in formData
        formData.append('datamodel', JSON.stringify(store.state.reportFields));
        fetch(api, {
            body: formData,
            method: "post",
            headers: {
                //"Content-Type": "multipart/form-data"
                "encType": "multipart/form-data"
            }
        })
            .then(response => response.json())
            .then(data => {
                store.commit(Mutations.SET_REPORT_DATA, data)
            });
    },
}