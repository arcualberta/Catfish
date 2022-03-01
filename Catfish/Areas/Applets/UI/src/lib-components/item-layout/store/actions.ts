import { ActionTree } from 'vuex';
import { State } from './state';
import { Mutations } from './mutations';
//import { Guid } from 'guid-typescript';

//Declare ActionTypes
export enum Actions {
    LOAD_ITEM = "LOAD_ITEM"
    
}

export const actions: ActionTree<State, any> = {

  [Actions.LOAD_ITEM](store) {
 
        const api = window.location.origin +
            `/applets/api/items/${store.state.id}`;
        console.log('Item Load API: ', api)

        fetch(api)
            .then(response => response.json())
            .then(data => {

              store.commit(Mutations.SET_ITEM, data);
            });
    },
   
}

