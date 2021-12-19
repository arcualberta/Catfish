import { ActionTree } from 'vuex';
import { State } from './state';
import { Mutations } from './mutations';
import { IndexingStatus } from '../models';

//Declare ActionTypes
export enum Actions {
    REINDEX_DATA = 'REINDEX_DATA',
    REINDEX_PAGES = 'REINDEX_PAGES',
    FETCH_REINDEX_STATUS = 'FETCH_REINDEX_STATUS'
}

export const actions: ActionTree<State, any> = {

    [Actions.REINDEX_DATA](store) {
        const api = window.location.origin +
            `/applets/api/reindex/data`;

        fetch(api, {
            method: 'POST'
        })
            .then(response => response.json())
            .then(data => {
                store.commit(Mutations.SET_REINDEX_DATA_STATUS, data)
            })
            .catch(error => {
                console.error('Data reindexing error:', error);
            });
    },

    [Actions.REINDEX_PAGES](store) {
        const api = window.location.origin +
            `/applets/api/reindex/pages`;

        fetch(api, {
            method: 'POST'
        })
            .then(response => response.json())
            .then(data => {
                store.commit(Mutations.SET_REINDEX_PAGE_STATUS, data)
            })
            .catch(error => {
                console.error('Page reindexing error:', error);
            });
    },

    [Actions.FETCH_REINDEX_STATUS](store) {
        const api = window.location.origin +
            `/applets/api/reindex/status`;

        fetch(api)
            .then(response => response.json())
            .then(data => {
                const status = data as IndexingStatus;
                store.commit(Mutations.SET_REINDEX_STATUS, status);

                if (status.dataIndexingInprogress || status.pageIndexingInprogress) {
                    //If an indexing is in-progress, check again in "ms" seconds
                    const checkBackDelayMilliSec = 5000;
                    new Promise(resolve => setTimeout(resolve, checkBackDelayMilliSec));
				}
            })
            .catch(error => {
                console.error('Fetch reindexing status error:', error);
            });
  },
}

