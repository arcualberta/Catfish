import { ActionTree } from 'vuex';
import { State } from './state';
import { Mutations } from './mutations';

//Declare ActionTypes
export enum Actions {
    LOAD_FORM = "LOAD_FORM",
    LOAD_SUBMISSIONS = "LOAD_SUBMISSIONS",
    ADD_CHILD_FORM = "ADD_CHILD_FORM"
}

export const actions: ActionTree<State, any> = {

    [Actions.LOAD_FORM](store) {
 
        const api = window.location.origin +
            `/applets/api/itemtemplates/getchildform/${store.state.itemTemplateId}/${store.state.formId}`;
        console.log('Form Load API: ', api)

        fetch(api)
            .then(response => response.json())
            .then(data => {
                //console.log('Data:\n', JSON.stringify(data));
                store.commit(Mutations.SET_FORM, data);
            })
            .catch(error => {
                console.error('Actions.LOAD_FORM Error: ', error);
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

    [Actions.ADD_CHILD_FORM](store) {
        const api = window.location.origin + `/applets/api/itemeditor/appendchildforminstance/${store.state.itemInstanceId}`;

        let formData = new FormData();
        formData.append('datamodel', JSON.stringify(store.state.form));

        fetch(api,
            {
                body: formData,
                method: "post"
            }).then(response =>
                response.json())
            .then(data => {
                console.log(JSON.stringify(data));

            })
            .catch(error => console.log(error));;
      
    },
}

