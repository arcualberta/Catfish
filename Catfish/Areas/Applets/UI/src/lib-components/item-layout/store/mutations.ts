import { MutationTree } from 'vuex';
import { State } from './state';
import { Guid } from 'guid-typescript'
//import { Item } from "../models/item"
import { ItemTemplate } from "../models/itemTemplate"
import { mutations as itemMutations } from '../../item-viewer/store/mutations';

//Declare MutationTypes
export enum Mutations {
  SET_ID = 'SET_ID',
 //SET_ITEM = 'SET_ITEM',
  //SET_ITEMS = 'SET_ITEMS',
  SET_TEMPLATE = 'SET_TEMPLATE',
    SET_FORM_IDS = 'SET_FORM_IDS',
    SET_TEMPLATE_ID ='SET_TEMPLATE_ID'
  
}

//Create a mutation tree that implement all mutation interfaces
export const mutations: MutationTree<State> = {

  [Mutations.SET_ID](state: State, payload: Guid) {
    state.id = payload;
    },
    [Mutations.SET_FORM_IDS](state: State, payload: String) {
        state.formIds = payload;
    },
  //[Mutations.SET_ITEM](state: State, payload: Item) {
  //  state.item = payload
  //},
    
  //[Mutations.SET_ITEMS](state: State, payload: Item[]) {
  //      state.items = payload
  //},
  [Mutations.SET_TEMPLATE](state: State, payload: ItemTemplate) {
        state.template = payload
    },
    [Mutations.SET_TEMPLATE_ID](state: State, payload: Guid) {
        state.templateId = payload
    },

    ...itemMutations
}
