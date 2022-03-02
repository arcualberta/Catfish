import { Guid } from 'guid-typescript'
import { ItemTemplate } from "../models/itemTemplate"
import {Item} from "../models/item"
//Declare State interface
export interface State {
  
    id: Guid | null;
    item: Item | null;
    items: Item[] | null;
    template: ItemTemplate | null;
    formIds: String | null;
    templateId: Guid | null;
}

export const state: State = {
  
    id: null,
    item: null,
    items: null,
    template: null,
    formIds: null,
    templateId: null
}
