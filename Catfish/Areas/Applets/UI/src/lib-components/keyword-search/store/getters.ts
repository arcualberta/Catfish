import { GetterTree } from 'vuex';
import { State } from './state';


export const getters: GetterTree<State, State> = {
    //  items: (state): Item[] | undefined => {
    //    return state.searchResult?.items
    //  },
    searchParamStorageKey: (state) => {
        return state.blockId?.toString() + "SearchParams";
    },
    isKeywordSelected: (state) => (containerIndex: number, fieldIndex: number, valueIndex: number) => {
        return state.keywordQueryModel?.
            containers[containerIndex]
            .fields[fieldIndex]
            .selected[valueIndex];
    }

}
