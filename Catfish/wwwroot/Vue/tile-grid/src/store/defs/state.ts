import { Guid } from "guid-typescript";

//Declare Tile interface which proxies the c# Tile class in Tile.cs
export interface Tile {
  id: Guid,
  title: string,
  subtitle: string,
  content: string,
  thumbnail: URL,
  date: Date,
  detailedViewUrl: URL
}

//Declare State interface
export interface State {
  items: Tile[]
}


export const state: State = {
  items: Array<Tile>()
}
