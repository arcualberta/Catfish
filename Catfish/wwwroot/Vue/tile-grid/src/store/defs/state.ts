import { Guid } from "guid-typescript";

//Declare Tile interface which proxies the c# Tile class in Tile.cs
export interface Tile {
  id: Guid,
  title: string,
  content: string,
  thumbnail: URL,
  created: Date,
  objectUrl: URL
}

//Declare State interface
export interface State {
  items: Tile[]
}


export const state: State = {
  items: Array<Tile>()
}
