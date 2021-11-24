import { GetterTree } from 'vuex';
import { State } from './state';


export const getters: GetterTree<State, State> = {
//  items: (state): Item[] | undefined => {
//    return state.searchResult?.items
//  },
    searchParamStorageKey:() =>{
        return "SearchParams";
    }

}
