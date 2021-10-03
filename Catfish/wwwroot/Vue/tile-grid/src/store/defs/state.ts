import { Guid } from "guid-typescript";

//Declare Tile interface which proxies the c# Tile class in Tile.cs
export interface Item {
  id: Guid,
  title: string,
  subtitle: string,
  categories: string[]
  content: string,
  thumbnail: URL,
  date: Date,
  detailedViewUrl: URL
}

export interface SearchResult {
  items: Item[],
  first: number,
  last: number,
  count: number
}

//Declare State interface
export interface State {
  searchResult: SearchResult | null
}


export const state: State = {
  searchResult: null,
}
