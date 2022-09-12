﻿import { ActionTree } from 'vuex';
import { State } from './state';
import { Mutations } from './mutations';
import { FlattenedFormFiledMutations } from '../../shared/store/flattened-form-field-mutations';

//Declare ActionTypes
export enum Actions {
    LOAD_ITEM = "LOAD_ITEM",
    GET_USER_ACTIONS = "GET_USER_ACTIONS",
    CHANGE_STATE = "CHANGE_STATE",
    SAVE = "SAVE",
    DELETE = "DELETE",
}

export const actions: ActionTree<State, any> = {

    [Actions.LOAD_ITEM](store) {

        const api = (store.state.siteUrl ? store.state.siteUrl : window.location.origin) +
            `/applets/api/items/${store.state.id}`;
        console.log('Item Load API: ', api)

        fetch(api)
            .then(response => response.json())
            .then(data => {
                store.commit(Mutations.SET_ITEM, data);
            });
    },
    [Actions.GET_USER_ACTIONS](store) {
        console.log("insude GET_USER_ACTIONS");
        const api = (store.state.siteUrl ? store.state.siteUrl : window.location.origin) +
            `/applets/api/items/getUserPermissions/${store.state.id}`;
        console.log('Permission Load API: ', api)

        fetch(api)
            .then(response => response.json())
            .then(data => {
                console.log(JSON.stringify(data))
                store.commit(Mutations.SET_USER_PERMISSIONS, data);
            });
    },

    [Actions.DELETE](store) {
        console.log("Delete Action Started");
        const api = (store.state.siteUrl ? store.state.siteUrl : window.location.origin) +
            `/applets/api/items/deleteItem/${store.state.id}`;
        console.log('Item Delete API: ', api)

        const item = store.state.item;
        //Validating the forms
        if (!item)
            return;

        fetch(api,
            {
                method: "post",
                headers: {
                    //"Content-Type": "multipart/form-data"
                    "encType": "multipart/form-data"
                }
            }).then(response => {
                //response.json()
                console.log(response.status)
                switch (response.status) {
                    case 200:
                        window.location.href = "/";
                        //alert("TODO: change me to redirect to home page.");
                        break;
                    case 401:
                        alert("Authorization failed.")
                        break;
                    case 404:
                        alert("Item not found.")
                        break;
                    case 500:
                        alert("Internal server error occurred.")
                        break;
                    default:
                        alert("Unknown error occurred.")
                        break;
                }
            })
            //.then(data => {
            //    console.log(JSON.stringify(data));
            //    store.commit(FlattenedFormFiledMutations.REMOVE_FIELD_CONTAINERS);
            //    //store.commit(Mutations.SET_ITEM, data);

            //})
            .catch(error => {
                console.log("error",error)
            });
    },

    [Actions.SAVE](store) {

        const api = (store.state.siteUrl ? store.state.siteUrl : window.location.origin) +
            `/applets/api/items/update/`;
        console.log('Item Update API: ', api)

        const item = store.state.item;

        //Validating the forms
        if (!item)
            return;

        const formData = new FormData();

        //Setting the serialized JSON form model to the datamodel variable in formData
        formData.append('item', JSON.stringify(item));

        //Adding all attachments uploaded to the files variable in formData
        for (const key in store.state.flattenedFileModels) {
            if (store.state.flattenedFileModels[key].length > 0) {
                store.state.flattenedFileModels[key].forEach(file => {
                    console.log("File: ", file.name)
                    formData.append('files', file);
                    formData.append('fileKeys', key);
                })
            }
        }

        fetch(api,
            {
                body: formData,
                method: "post",
                headers: {
                    //"Content-Type": "multipart/form-data"
                    "encType": "multipart/form-data"
                }
            }).then(response =>
                response.json())
            .then(data => {
                console.log(JSON.stringify(data));
                store.commit(FlattenedFormFiledMutations.REMOVE_FIELD_CONTAINERS);
                store.commit(Mutations.SET_ITEM, data);

            })
            .catch(error => {
                console.log(error)
            });
    },
}
