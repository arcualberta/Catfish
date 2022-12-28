
import { eFieldType } from "../../shared/constants";

export interface SearchFieldDefinition{
    name: string,
    label: string,
    type: eFieldType,
    options: string[]
}

export interface SolrEntryType {
    name: string,
    label: string,
    entityType: number
}

export enum eSolrBooleanOperators {
    AND = "AND",
    OR = "OR"
}

export enum eUiMode{
    Default = 0,
    Raw,
    Curated
}

export interface SolrResultEntry{
    id: string
    //data: Record<string, SolrFieldData[]>[]
    data: Record<string, Object>[]
}

export interface SolrFieldData{
    key: string
    value: string
}

export interface SearchResult {
    totalMatches: number,
    offset: number,
    itemsPerPage: number,
    resultEntries: SolrResultEntry[]
}

/* solr model from previous version -- we might or might not need them */

/*
export interface ResultItem {
    id: Guid;
    title: string;
    subtitle: string;
    categories: string[];
    content: string;
    thumbnail: URL;
    date: Date;
    detailedViewUrl: URL;
    rootFormInstaceId: Guid;
    solrFields: SolrField[] | null;
}

export interface SearchOutput {
    items: ResultItem[];
    first: number;
    last: number;
    count: number;
}

export interface SearchParams {
    pageId: Guid;
    blockId: Guid;
    keywords: KeywordQueryModel;
    offset: number;
    max: number;
}

export interface KeywordSource {
    pageId: Guid;
    blockId: Guid;
}

export interface KeywordField {
    aggregation: number;
    id: Guid;
    name: string;
    values: string[];
    selected: boolean[];
}

export interface KeywordFieldContainer {
    aggregation: number;
    id: Guid;
    containerType: number;
    name: string;
    fields: KeywordField[];
}

export interface KeywordQueryModel {
    aggregation: number;
    containers: KeywordFieldContainer[];
}

export interface KeywordIndex {
    containerIndex: number;
    fieldIndex: number;
    valueIndex: number
};

export interface Keyword {
    index: KeywordIndex;
    value: string;
}
export interface SolrField {
    
    //MR May 10 2022
    solrFieldId: string | null;
    fieldContent: string | null;
}



*/
