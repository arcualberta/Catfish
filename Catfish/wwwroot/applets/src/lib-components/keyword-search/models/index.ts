﻿import { Guid } from "guid-typescript";

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

export interface SearchOutput {
  items: Item[],
  first: number,
  last: number,
  count: number
}

export interface SearchParams {
  pageId: Guid,
  blockId: Guid,
  keywords: string[],
  offset: number,
  max: number
}