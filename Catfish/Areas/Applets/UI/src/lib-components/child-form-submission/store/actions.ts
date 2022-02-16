import { ActionTree } from 'vuex';

import { State } from './state';
import { Mutations as ChildFormMutations } from './mutations'
import { Mutations } from '../../form-submission/store/mutations'
import { clearForm, FlattenedFormFiledState } from '../../shared/store/form-submission-utils'
import { validateFields } from '../../shared/store/form-validators'
import { Guid } from 'guid-typescript';
import { FieldContainer } from '../../shared/models/fieldContainer';

//Declare ActionTypes
export enum Actions {
    LOAD_FORM = "LOAD_FORM",
    LOAD_SUBMISSIONS = "LOAD_SUBMISSIONS",
    LOAD_RESPONSE_FORM = "LOAD_RESPONSE_FORM",
    SUBMIT_CHILD_FORM = "SUBMIT_CHILD_FORM",
    SUBMIT_CHILD_RESPONSE_FORM = "SUBMIT_CHILD_RESPONSE_FORM",
    DELETE_CHILD_RESPONSE_INSTANCE = "DELETE_CHILD_RESPONSE_INSTANCE",
    DELETE_CHILD_INSTANCE = "DELETE_CHILD_INSTANCE"
}

export const actions: ActionTree<State, any> = {

    [Actions.LOAD_FORM](store) {

        const api = window.location.origin +
            `/applets/api/itemeditor/getchildform/${store.state.itemInstanceId}/${store.state.formId}`;

        fetch(api)
            .then(response => response.json())
            .then(data => {
                store.commit(Mutations.SET_FORM, data);
            })
            .catch(error => {
                console.error('Actions.LOAD_FORM Error: ', error);
            });
    },
    [Actions.LOAD_RESPONSE_FORM](store) {

        const api = window.location.origin +
            `/applets/api/itemeditor/getchildform/${store.state.itemInstanceId}/${store.state.childResponseFormId}`;

        fetch(api)
            .then(response => response.json())
            .then(data => {
                store.commit(ChildFormMutations.SET_RESPONSE_FORM, data);
            })
            .catch(error => {
                console.error('Actions.LOAD_FORM Error: ', error);
            });
    },
    [Actions.LOAD_SUBMISSIONS](store) {

        const api = window.location.origin +
            `/applets/api/itemeditor/getchildformsubmissions/${store.state.itemInstanceId}/${store.state.formId}`;
        console.log('Child Submissions Load API: ', api)

        fetch(api)
            .then(response => response.json())
            .then(data => {
                store.commit(ChildFormMutations.SET_SUBMISSIONS, data);
            })
            .catch(error => {
                console.error('Submission loading error:', error);
            });
    },

    [Actions.SUBMIT_CHILD_FORM](store) {

        //Validating the form
        if (!store.state.form || !validateFields(store.state.form))
            return;

        store.commit(Mutations.SET_SUBMISSION_STATUS, "InProgress");

        const api = window.location.origin + `/applets/api/itemeditor/appendchildforminstance/${store.state.itemInstanceId}`;

        const formData = new FormData();
        formData.append('datamodel', JSON.stringify(store.state.form));


        fetch(api,
            {
                body: formData,
                method: "post"
            })
            .then(response =>
                response.json())
            .then(data => {
                const flattenModel: FlattenedFormFiledState = {
                    flattenedOptionModels: store.state.flattenedOptionModels,
                    flattenedTextModels: store.state.flattenedTextModels,
                };
                //clear the form content
                clearForm(flattenModel);

                store.commit(ChildFormMutations.APPEND_CHILD_INSTANCE, data);
                store.commit(Mutations.SET_SUBMISSION_STATUS, "Success");

            })
            .catch(error => {
                store.commit(Mutations.SET_SUBMISSION_STATUS, "Fail");
                console.log(error)
            });
    },

    [Actions.SUBMIT_CHILD_RESPONSE_FORM](store, parentId: Guid | undefined) {

        //Validating the form
        if (!store.state.childResponseForm || !validateFields(store.state.childResponseForm))
            return;

        const api = window.location.origin + `/applets/api/itemeditor/appendchildforminstance/${store.state.itemInstanceId}`;

        const formData = new FormData();
        formData.append('datamodel', JSON.stringify(store.state.childResponseForm));

        if (parentId)
            formData.append('parentId', parentId.toString());

        fetch(api,
            {
                body: formData,
                method: "post"
            })
            .then(response =>
                response.json())
            .then(data => {
                //console.log("Response Data: \n", JSON.stringify(data))
                store.commit(ChildFormMutations.APPEND_CHILD_RESPONSE_INSTANCE, data)
                const flattenModel: FlattenedFormFiledState = {
                    flattenedOptionModels: store.state.flattenedOptionModels,
                    flattenedTextModels: store.state.flattenedTextModels,
                };
                //clear the form content
                clearForm(flattenModel);
            })
            .catch(error => {
                store.commit(Mutations.SET_SUBMISSION_STATUS, "Fail");
                console.log(error)
            });
    },

    [Actions.DELETE_CHILD_RESPONSE_INSTANCE](store, payload: FieldContainer) {

        const api = window.location.origin + `/applets/api/itemeditor/deleteChildForm/${store.state.itemInstanceId}/${payload.id}?parentId=${payload.parentId}`;

        fetch(api,
            {
                method: "post"
            })
            .then(response =>
                response.json())
            .then(data => {
                if (data.id) {
                    console.log("deleteChildForm response received")
                    store.commit(ChildFormMutations.DELETE_CHILD_RESPONSE_INSTANCE, payload);
				}
            })
            .catch(error => {
                store.commit(Mutations.SET_SUBMISSION_STATUS, "Fail");
                console.log(error)
            });        
    },
    [Actions.DELETE_CHILD_INSTANCE](store, payload: FieldContainer) {

        const api = window.location.origin + `/applets/api/itemeditor/deleteChildForm/${store.state.itemInstanceId}/${payload.id}`;

        fetch(api,
            {
                method: "post"
            })
            .then(response =>
                response.json())
            .then(data => {
                if (data.id) {
                    console.log("deleteChildForm response received")
                    store.commit(ChildFormMutations.DELETE_CHILD_RESPONSE_INSTANCE, payload);
                }
            })
            .catch(error => {
                store.commit(Mutations.SET_SUBMISSION_STATUS, "Fail");
                console.log(error)
            });

        

    },
}


