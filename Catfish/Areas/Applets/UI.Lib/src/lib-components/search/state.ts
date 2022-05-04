import { SearchOutput, KeywordQueryModel } from './models'

//Declare State interface
export interface State {
    keywordQueryModel: KeywordQueryModel | null;
    freeSearchText: string | null;
    offset: number;
    max: number;
    searchResult: SearchOutput | null;
}

export const state: State = {
    keywordQueryModel: null,
    freeSearchText: null,
    offset: 0,
    max: 25,
    searchResult: null,
}


