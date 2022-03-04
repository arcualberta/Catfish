import { Guid } from 'guid-typescript';
import { GetterTree } from 'vuex';
import { State } from './state';

export const getters: GetterTree<State, State> = {
   
    dataItem: (state) => (itemTemplateId: Guid) => {
        return (state.item?.dataContainer?.$values?.filter(dc => dc.templateId === itemTemplateId)[0]);
    },
    field: (state) => (itemTemplateId: Guid, fieldId: Guid) => {

        return (state.item?.dataContainer?.$values?.filter(dc => dc.templateId === itemTemplateId)[0])?.fields.$values?.filter(fd => fd.id === fieldId);
    }
}
