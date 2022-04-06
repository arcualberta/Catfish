import { Guid } from 'guid-typescript'
import { SearchOutput } from '../models'
import { KeywordQueryModel } from '../models/keywords'
import { DataAttribute, QueryParameter } from '../../shared/props'

//Declare State interface
export interface State {
    keywordQueryModel: KeywordQueryModel | null;
    searchResult: SearchOutput | null;
    offset: number;
    max: number;
    pageId: Guid | null;
    blockId: Guid | null;
    freeSearchText: string | null;
    dataAttributes: DataAttribute | null;
    wueryParameters: QueryParameter | null;
}

export const state: State = {
    keywordQueryModel: null,
    searchResult: null,
    offset: 0,
    max: 25,
    pageId: null,
    blockId: null,
    freeSearchText: null,
    dataAttributes: null,
    wueryParameters: null
}
