import { GetterTree } from 'vuex';
import { State } from './state';
import { Guid } from 'guid-typescript';

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
    },
    getItem: (state) => (itemId: Guid) => {
        //NOT GET IN HERE!!!!!
        console.log("inside getter getitem: " + itemId)
       return  state.searchResult?.items.filter(it => it.id === itemId);
    }
}
