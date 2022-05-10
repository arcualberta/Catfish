import { Guid } from 'guid-typescript'

//Declare State interface
export interface State {
	pageId: Guid | null;
	blockId: Guid | null;
	dataServiceApiRoot: string | null;
	solrServiceApiRoot: string | null;
	pageServiceApiRoot: string | null;

	//MR May 10 2022
	collectionId: Guid | null;
	templateId: Guid | null;
	groupId: Guid | null;
}

export const state: State = {
	pageId: null,
	blockId: null,
	dataServiceApiRoot: null,
	solrServiceApiRoot: null,
	pageServiceApiRoot: null,
	collectionId: null,
	templateId: null,
	groupId: null
}

