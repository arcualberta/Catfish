//import { Guid } from 'guid-typescript'

import { IndexingStatus } from "../models";

//Declare State interface
export interface State {
	indexingStatus: IndexingStatus;
}

export const state: State = {
	indexingStatus: {
			pageIndexingInprogress: false,
			dataIndexingInprogress: false
	} as IndexingStatus
}
