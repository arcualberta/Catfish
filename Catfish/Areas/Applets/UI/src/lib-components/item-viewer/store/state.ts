import { Guid } from 'guid-typescript'
import {Item} from "../models/item"
//Declare State interface
export interface State {
  
    Id: Guid | null;
    item: Item | null;
  
}

export const state: State = {
  
    Id: null,
    item: null
}
