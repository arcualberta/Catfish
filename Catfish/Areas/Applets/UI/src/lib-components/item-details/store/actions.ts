import { ActionTree } from 'vuex';
import { State } from './state';
import { Mutations } from './mutations';

//Declare ActionTypes
export enum Actions {
    LOAD_ITEM = "LOAD_ITEM",
    GET_USER_ACTIONS = "GET_USER_ACTIONS",
    CHANGE_STATE = "CHANGE_STATE",
    SAVE = "SAVE",
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
    [Actions.GET_USER_ACTIONS](store) {

        const api = window.location.origin +
            `/applets/api/items/getUserPermissions/${store.state.id}`;
        console.log('Permission Load API: ', api)

        fetch(api)
            .then(response => response.json())
            .then(data => {
                console.log(JSON.stringify(data))
                store.commit(Mutations.SET_USER_PERMISSIONS, data);
            });
    },
    [Actions.SAVE](store) {

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

