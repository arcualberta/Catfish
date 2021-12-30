import { ActionTree } from 'vuex';
import { State } from './state';
import { Mutations } from './mutations';
import { eIndexingStatus, IndexingStatus } from '../models';

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

        store.commit(Mutations.SET_REINDEX_DATA_STATUS, eIndexingStatus.InProgress)

        fetch(api, { method: 'POST' })
            .then(response => response.json())
            .then(data => {
                store.commit(Mutations.SET_REINDEX_DATA_STATUS, data as eIndexingStatus);
            })
            .catch(error => {
                console.error('Data reindexing error:', error);
            });
    },

    [Actions.REINDEX_PAGES](store) {
        const api = window.location.origin +
            `/applets/api/reindex/pages`;

        store.commit(Mutations.SET_REINDEX_PAGE_STATUS, eIndexingStatus.InProgress);

        fetch(api, {
            method: 'POST'
        })
            .then(response => response.json())
            .then(data => {
                store.commit(Mutations.SET_REINDEX_PAGE_STATUS, data as eIndexingStatus)
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
                store.commit(Mutations.SET_REINDEX_STATUS, data as IndexingStatus);
            })
            .catch(error => {
                console.error('Fetch reindexing status error:', error);
            });
  },
}

