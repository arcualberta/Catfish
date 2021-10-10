import { Guid } from "guid-typescript";

export interface KeywordSource {
  pageId: Guid,
  blockId: Guid
}

export interface KeywordField {
  aggregation: number,
  id: Guid,
  name: string,
  values: string[]
}

export interface KeywordFieldContainer {
  aggregation: number,
  id: Guid,
  containerType: number,
  name: string,
  fields: KeywordField[]
}

export interface SortKeywordsInFields {
  aggregation: number,
  containers: KeywordFieldContainer[]
}
