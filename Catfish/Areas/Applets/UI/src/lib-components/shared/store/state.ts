import { Guid } from 'guid-typescript'
import { Block, Page } from '../models/cmsModels'

//Declare State interface
export interface State {
	model: Block | Page | null;
	pageId: Guid | null;
	blockId: Guid | null;
}

export const state: State = {
	model: null,
	pageId: null,
	blockId: null
}

