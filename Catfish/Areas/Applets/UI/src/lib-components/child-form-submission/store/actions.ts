import { ActionTree } from 'vuex';
import { State } from './state';
import { Mutations } from './mutations';

//Declare ActionTypes
export enum Actions {
    LOAD_FORM = "LOAD_FORM",
    LOAD_SUBMISSIONS = "LOAD_SUBMISSIONS"
}

export const actions: ActionTree<State, any> = {

    [Actions.LOAD_FORM](store) {
 
        const api = window.location.origin +
            `/applets/api/itemtemplates/getchildform/${store.state.itemTemplateId}/${store.state.formId}`;
        console.log('Form Load API: ', api)

        fetch(api)
            .then(response => response.json())
            .then(data => {

                store.commit(Mutations.SET_FORM, data);
            })
            .catch(error => {
                console.error('Child form loading error:', error);
            });
;
    },
    [Actions.LOAD_SUBMISSIONS](store) {

        const api = window.location.origin +
            `/applets/api/items/getchildforms/${store.state.itemInstanceId}/${store.state.formId}`;
        console.log('NOT IMPLEMENTED YET::Child Submission Load API: ', api)

    //    fetch(api)
    //        .then(response => response.json())
    //        .then(data => {
    //            store.commit(Mutations.SET_SUBMISSIONS, data);
    //        })
    //        .catch(error => {
    //            console.error('Submission loading error:', error);
    //        });
    },
}

