import { Guid } from 'guid-typescript';
import { GetterTree } from 'vuex';
import { State } from './state';

export const getters: GetterTree<State, State> = {
  rootDataItem: state => {
    return state.item?.dataContainer.filter(dc => dc.isRoot)[0];
    },

    metadataSet: (state) => (id: Guid) => {
        console.log("metadataset getter id: " + JSON.stringify(id));
        return state.item?.metadataSets?.find(ms => ms.templateId === id);
    }
}
