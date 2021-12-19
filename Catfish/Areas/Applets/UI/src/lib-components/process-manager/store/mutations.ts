import { MutationTree } from 'vuex';
import { State } from './state';

//Declare MutationTypes
export enum Mutations {
    MY_MUTATION = 'MY_MUTATION'
}

//Create a mutation tree that implement all mutation interfaces
export const mutations: MutationTree<State> = {

    [Mutations.MY_MUTATION](state: State, payload: any) {
        console.log(JSON.stringify(state), JSON.stringify(payload))
    }


}
