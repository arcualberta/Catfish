﻿import { Guid } from "guid-typescript";
import { Item } from '../../shared/models'

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