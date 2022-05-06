import { Guid } from 'guid-typescript'

//Declare State interface
export interface State {
	pageId: Guid | null;
	blockId: Guid | null;
	dataServiceApiRoot: string | null;
	solrServiceApiRoot: string | null;
	pageServiceApiRoot: string | null;
}

export const state: State = {
	pageId: null,
	blockId: null,
	dataServiceApiRoot: null,
	solrServiceApiRoot: null,
	pageServiceApiRoot: null,
}

