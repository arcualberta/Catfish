import { MutationTree } from 'vuex';
import { State } from './state';
import { Guid } from 'guid-typescript'
import { Item } from "../models/item"

//Declare MutationTypes
export enum Mutations {
    SET_ID = 'SET_ID',
    SET_TEMPLATE='SET_item'
}

//Create a mutation tree that implement all mutation interfaces
export const mutations: MutationTree<State> = {

  [Mutations.SET_ID](state: State, payload: Guid) {
    state.Id = payload;
    // console.log("template id : " + state.Id)
  },
  [Mutations.SET_TEMPLATE](state: State, payload: Item) {
    state.item = payload
    // console.log("template ID: " + state.template.id);
    // console.log("template name: " + state.template.templateName);
    // console.log("field length: " + state.template.dataContainer[0].fields.length)
  }

}
