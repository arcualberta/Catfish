import { ActionTree } from 'vuex';
import { State } from './state';
import { Mutations } from './mutations';

export enum Actions {
    LOAD_DATA = 'LOAD_DATA',
    INIT_FILTER = 'INIT_FILTER',
    FRESH_SEARCH = 'FRESH_SEARCH'
}
export const actions: ActionTree<State, any> = {
    [Actions.LOAD_DATA](store) {

        //console.log('Store: ', JSON.stringify(store.state))

        const api = window.location.origin +
            `/applets/api/reports/group/${store.state.groupId}/template/${store.state.itemTemplateID}/collection/${store.state.collectionID}`;
        console.log('reports Load API: ', api)

        fetch(api)
            .then(response => response.json())
            .then(data => {
                store.commit(Mutations.SET_REPORT_DATA, data)

            });
    },

    
}
