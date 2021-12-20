//import { Guid } from 'guid-typescript'

import { eIndexingStatus, IndexingStatus } from "../models";

//Declare State interface
export interface State {
	indexingStatus: IndexingStatus;
}

export const state: State = {
	indexingStatus: {
		pageIndexingStatus: eIndexingStatus.Ready,
		dataIndexingStatus: eIndexingStatus.Ready
	} as IndexingStatus
}
