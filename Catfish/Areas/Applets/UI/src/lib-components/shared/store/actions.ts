import { ActionTree } from 'vuex';
import { State } from './state';
import { Mutations } from './mutations';

//Declare ActionTypes
export enum Actions {
    LOAD_BLOCK = 'LOAD_BLOCK',
    LOAD_PAGE = 'LOAD_PAGE'
   
}

export const actions: ActionTree<State, any> = {

    [Actions.LOAD_BLOCK](store) {

        if (!store.state.pageId)
            console.error("Page ID is null. It must be a valid GUID");

        if (!store.state.blockId)
            console.error("Block ID is null. It must be a valid GUID");

        const api = window.location.origin
            + `/applets/api/content/page/${store.state.pageId}/block/${store.state.blockId}`;
        console.log('LOAD_BLOCK API: ', api);

        fetch(api, { method: 'GET' })
            .then(response => response.json())
            .then(data => {
                store.commit(Mutations.SET_MODEL, data)
            })
            .catch(error => {
                console.error('LOAD_BLOCK error:', error);
            });
    },

    [Actions.LOAD_PAGE](store) {
        if (!store.state.pageId)
            throw new Error("Page ID is null. It must be a valid GUID");

        const api = window.location.origin
            + `/applets/api/content/page/${store.state.pageId}`;
        console.log('LOAD_PAGE API: ', api);

        fetch(api, { method: 'GET' })
            .then(response => response.json())
            .then(data => {
                store.commit(Mutations.SET_MODEL, data)
            })
            .catch(error => {
                console.error('LOAD_PAGE error:', error);
            });
    }
    
}

