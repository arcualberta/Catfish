import { Guid } from "guid-typescript";

export interface ResultItem {
    id: Guid;
    title: string;
    subtitle: string;
    categories: string[];
    content: string;
    thumbnail: URL;
    date: Date;
    detailedViewUrl: URL;

    //MR May 10 2022
    itemFields: ItemField[] | null;
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
export interface ItemField {
    
    //MR May 10 2022
    solrFieldId: string | null;
    fieldContent: string | null;
}
