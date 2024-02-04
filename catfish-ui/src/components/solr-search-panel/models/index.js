export var eSolrBooleanOperators;
(function (eSolrBooleanOperators) {
    eSolrBooleanOperators["AND"] = "AND";
    eSolrBooleanOperators["OR"] = "OR";
})(eSolrBooleanOperators || (eSolrBooleanOperators = {}));
export var eUiMode;
(function (eUiMode) {
    eUiMode[eUiMode["Default"] = 0] = "Default";
    eUiMode[eUiMode["Raw"] = 1] = "Raw";
    eUiMode[eUiMode["Curated"] = 2] = "Curated";
})(eUiMode || (eUiMode = {}));
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
//# sourceMappingURL=index.js.map