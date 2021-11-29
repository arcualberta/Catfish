import { ActionTree } from 'vuex';
import { State } from './state';
import { Mutations } from './mutations';

//Declare ActionTypes
export enum Actions {
    LOAD_TEMPLATE = "LOAD_TEMPLATE",
    SET_ID = "SET_ID"
}

export const actions: ActionTree<State, any> = {

    [Actions.LOAD_TEMPLATE](store) {

        //console.log('Store: ', JSON.stringify(store.state))
       
        const api = window.location.origin +
            `/applets/api/itemtemplates/${store.state.Id}`;
        console.log('Keyword Load API: ', api)

        fetch(api)
            .then(response => response.json())
            .then(data => {
                //store.commit(Mutations.SET_KEYWORDS, data)
                 console.log(JSON.stringify(data))

            });
    },

    [Actions.SET_ID](store, payload) {

        store.commit(Mutations.SET_ID, payload);
    },

}

