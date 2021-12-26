
export enum eIndexingStatus { InProgress = 1, Ready }

export interface IndexingStatus {
	pageIndexingStatus: eIndexingStatus;
	dataIndexingStatus: eIndexingStatus;
}
