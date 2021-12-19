import { ActionTree } from 'vuex';
import { State } from './state';

//Declare ActionTypes
export enum Actions {
    MY_ACTION = 'MY_ACTION'
}

export const actions: ActionTree<State, any> = {

    [Actions.MY_ACTION](store) {
        console.log('Store: ', JSON.stringify(store.state))
    }
}

