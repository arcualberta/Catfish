import { MutationTree } from 'vuex';
import { State } from './state';

//Declare MutationTypes
export enum Mutations {
  INIT_APPLET = 'INIT_APPLET'
}

//Create a mutation tree that implement all mutation interfaces
export const mutations: MutationTree<State> = {

  [Mutations.INIT_APPLET](state: State, payload: State) {
    state.pageId = payload.pageId;
    state.blockId = payload.blockId;
    state.appletName = payload.appletName;
    console.log("Payload: ", payload)
  }
}
