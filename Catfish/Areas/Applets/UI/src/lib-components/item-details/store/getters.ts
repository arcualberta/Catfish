import { Guid } from 'guid-typescript';
import { GetterTree } from 'vuex';
import { State } from './state';

export const getters: GetterTree<State, State> = {
    metadataSet: (state) => (id: Guid) => {
        return state.item?.metadataSets?.$values?.find(ms => ms.templateId === id);
    },
}
