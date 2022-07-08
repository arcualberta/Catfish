import { Guid } from 'guid-typescript'
import {ItemTemplate} from "../models/itemTemplate"
//Declare State interface
export interface State {
  
    Id: Guid | null;
    template: ItemTemplate | null;
  
}

export const state: State = {
  
    Id: null,
    template: null
}
