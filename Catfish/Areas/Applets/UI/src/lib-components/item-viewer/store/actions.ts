import { ActionTree } from 'vuex';
import { State } from './state';
import { Mutations } from './mutations';
import { Guid } from 'guid-typescript';

//Declare ActionTypes
export enum Actions {
    LOAD_ITEM = "LOAD_ITEM",
    CHANGE_STATE="CHANGE_STATE"
}

export const actions: ActionTree<State, any> = {

  [Actions.LOAD_ITEM](store) {
 
        const api = window.location.origin +
            `/applets/api/itemeditor/${store.state.id}`;
        console.log('Item Load API: ', api)

        fetch(api)
            .then(response => response.json())
            .then(data => {

              store.commit(Mutations.SET_ITEM, data);
            });
    },
    [Actions.CHANGE_STATE](store, payload: { itemId: Guid, state: string }) {

        console.log(JSON.stringify(store.state));

        const api = window.location.origin +
            `/applets/api/itemeditor/${payload.itemId}/changestate/${payload.state}`;
        console.log('Item Load API: ', api)

        //fetch(api)
        //    .then(response => response.json())
        //    .then(data => {
        //        console.log(JSON.stringify(data));
        //        store.commit(Mutations.CHANGE_STATE, payload);
        //    });
    },
}

