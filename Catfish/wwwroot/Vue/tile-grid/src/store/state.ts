import { Tile } from "./tile";

//Declare State interface
export interface State {
  tiles: Tile[]
}

//Define the state object
export const state: State = {
  tiles: Array<Tile>()
}


