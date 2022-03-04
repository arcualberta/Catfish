import { Guid } from 'guid-typescript'
import { ItemTemplate } from "../models/itemTemplate"
import { Item } from "../../item-viewer/models/item"
import {State as ItemStateInterface,  state as itemState} from '../../item-viewer/store/state'
//Declare State interface
export interface State extends ItemStateInterface{
  
   // id: Guid | null;
   // item: Item | null;
    items: Item[] | null;
    template: ItemTemplate | null;
    formIds: String | null;
    templateId: Guid | null;
}

export const state: State = {
  
   // id: null,
    //item: null,
    items: null,
    template: null,
    formIds: null,
    templateId: null,
    ...itemState
}
