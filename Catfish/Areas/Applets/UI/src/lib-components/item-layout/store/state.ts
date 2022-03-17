////import { Guid } from 'guid-typescript'
////import { ItemTemplate } from "../models/itemTemplate"
import { State as ItemStateInterface, state as itemState } from '../../item-viewer/store/state'

//Declare State interface
export interface State extends ItemStateInterface {
    //items: Item[] | null;
    //formIds: string | null;
//    template: ItemTemplate | null;
//    templateId: Guid | null;
}

export const state: State = {
    //items: null,
    //formIds: null,
    //template: null,
    //templateId: null,
    ...itemState
}
