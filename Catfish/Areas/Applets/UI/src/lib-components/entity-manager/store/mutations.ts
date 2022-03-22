import { MutationTree } from 'vuex';
import { State } from './state';
import { mutations as itemMutations } from '../../item-viewer/store/mutations';


//Create a mutation tree that implement all mutation interfaces
export const mutations: MutationTree<State> = {
    ...itemMutations
}
