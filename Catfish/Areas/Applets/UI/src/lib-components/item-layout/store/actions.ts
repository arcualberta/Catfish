import { ActionTree } from 'vuex';
import { State } from './state';
import {actions as itemActions } from "../../item-viewer/store/actions"
//import { Mutations } from './mutations';
//import { Guid } from 'guid-typescript';

//Declare ActionTypes
export enum Actions{
    LOAD_ITEM = "LOAD_ITEM",
    LOAD_TEMPLATE = "LOAD_TEMPLATE",
   // LOAD_ITEMS="LOAD_ITEMS"
    
}

export const actions: ActionTree<State, any> = {
    //[Actions.LOAD_TEMPLATE](store) {

    //    const api = window.location.origin +
    //        `/applets/api/itemtemplates/${store.state.Id}`;
    //    //console.log('Keyword Load API: ', api)

    //    fetch(api)
    //        .then(response => response.json())
    //        .then(data => {

    //            store.commit(Mutations.SET_TEMPLATE, data);
    //            //console.log("Loaded Template datacontainer: " + JSON.stringify(store.state.template?.dataContainer))
    //            // console.log("Datacontainer count: " + store.state.template?.dataContainer.length)
    //        });
    //},
  //[Actions.LOAD_ITEM](store) {
 
  //      const api = window.location.origin +
  //          `/applets/api/items/${store.state.id}`;
  //      console.log('Item Load API: ', api)

  //      fetch(api)
  //          .then(response => response.json())
  //          .then(data => {

  //            store.commit(Mutations.SET_ITEM, data);
  //          });
  //  },

    //[Actions.LOAD_ITEMS](store) {

    //    //const api = window.location.origin +
    //    //    `/applets/api/items/getItems/${store.state.templateId}/${store.state.formIds}`;
    //    const api = window.location.origin +
    //        `/applets/api/items/getItems/${store.state.templateId}`;
    //    console.log('Load Items API: ', api)

    //    fetch(api)
    //        .then(response => response.json())
    //        .then(data => {

    //            store.commit(Mutations.SET_ITEMS, data);
    //        });
    //},
   ...itemActions
}

