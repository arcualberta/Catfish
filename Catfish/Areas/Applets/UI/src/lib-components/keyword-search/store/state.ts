import { Guid } from 'guid-typescript'
import { SearchOutput } from '../models'
import { KeywordQueryModel } from '../models/keywords'
import { DataAttribute, QueryParameter } from '../../shared/props'
import { State as ItemViewerState, state as itemViewerState } from '../../item-viewer/store/state'

export enum ePage { Home = "Home", List = "List", Details = "Details" }

//Declare State interface
export interface State extends ItemViewerState {
    keywordQueryModel: KeywordQueryModel | null;
    searchResult: SearchOutput | null;
    offset: number;
    max: number;
    pageId: Guid | null;
    blockId: Guid | null;
    freeSearchText: string | null;
    dataAttributes: DataAttribute | null;
    queryParameters: QueryParameter | null;
    activePage: ePage;
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
    queryParameters: null,
    activePage: ePage.Home,
    ...itemViewerState
}
