import { Guid } from 'guid-typescript'

import { SearchOutput, KeywordQueryModel } from './models'
import { State as BaseState, state as baseState } from '../shared/store/state'

//Declare State interface
export interface State extends BaseState {
    keywordQueryModel: KeywordQueryModel | null;
    freeSearchText: string | null;
    offset: number;
    max: number;
    searchResult: SearchOutput | null;
    pageId: Guid | null;
    blockId: Guid | null;
}

export const state: State = {
    ...baseState,
    keywordQueryModel: null,
    freeSearchText: null,
    offset: 0,
    max: 25,
    searchResult: null,
}


