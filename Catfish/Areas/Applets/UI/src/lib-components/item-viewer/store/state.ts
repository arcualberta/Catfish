import { Guid } from 'guid-typescript'
import {Item} from "../models/item"
//Declare State interface
export interface State {
  
    id: Guid | null;
    item: Item | null;
  
}

export const state: State = {
  
    id: null,
    item: null
}
