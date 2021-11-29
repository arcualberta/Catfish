import { MutationTree } from 'vuex';
import { State } from './state';
import { Guid } from 'guid-typescript'

//Declare MutationTypes
export enum Mutations {
    SET_ID = 'SET_ID',
    
}

//Create a mutation tree that implement all mutation interfaces
export const mutations: MutationTree<State> = {

    [Mutations.SET_ID](state: State, payload: Guid) {
        state.Id = payload;
        console.log("template id : " + state.Id)
    }

}
