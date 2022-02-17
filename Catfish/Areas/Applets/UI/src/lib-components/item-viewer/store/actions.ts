import { ActionTree } from 'vuex';
import { State } from './state';
import { Mutations } from './mutations';
import { Guid } from 'guid-typescript';

//Declare ActionTypes
export enum Actions {
    LOAD_ITEM = "LOAD_ITEM",
    CHANGE_STATE="CHANGE_STATE"
}

export const actions: ActionTree<State, any> = {

  [Actions.LOAD_ITEM](store) {
 
        const api = window.location.origin +
            `/applets/api/itemeditor/${store.state.id}`;
        console.log('Item Load API: ', api)

        fetch(api)
            .then(response => response.json())
            .then(data => {

              store.commit(Mutations.SET_ITEM, data);
            });
    },
    [Actions.CHANGE_STATE](store, payload: Guid ) {

        console.log(JSON.stringify(store.state));

        const api = window.location.origin +
            `/applets/api/itemeditor/deleteItem/${payload}`;
        console.log('Item Load API: ', api)

        fetch(api,
            {
                method: "post"
            })
            .then(response => {
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
            ////.then(data => {
            ////    console.log(data);
            ////    //if (data.status == 200) {
            ////    //    console.log("status ok "  + data.status);
            ////    //    //window.location.href = window.location.origin;
            ////    //} else {
            ////    //    alert("HTTP response return status code " + data.status);
            ////    //}
            ////})
            .catch(error => {
                alert("Unknown error occurred.")
                console.log(error)
            });
       
    },
}

