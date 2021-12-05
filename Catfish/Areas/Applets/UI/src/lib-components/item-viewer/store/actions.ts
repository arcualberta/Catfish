import { ActionTree } from 'vuex';
import { State } from './state';
import { Mutations } from './mutations';

//Declare ActionTypes
export enum Actions {
  LOAD_ITEM = "LOAD_ITEM",
    SET_ID = "SET_ID"
}

export const actions: ActionTree<State, any> = {

  [Actions.LOAD_ITEM](store) {
 
        const api = window.location.origin +
            `/applets/api/items/${store.state.Id}`;
        //console.log('Keyword Load API: ', api)

        fetch(api)
            .then(response => response.json())
            .then(data => {
                
                store.commit(Mutations.SET_TEMPLATE, data);
               console.log("Loaded datacontainer: " + JSON.stringify(store.state.template?.dataContainer))
               // console.log("Datacontainer count: " + store.state.template?.dataContainer.length)
            });
    },

    [Actions.SET_ID](store, payload) {

        store.commit(Mutations.SET_ID, payload);
    },

}

