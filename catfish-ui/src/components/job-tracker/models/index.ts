import { Guid } from "guid-typescript";

export interface JobRecord{
    id: Guid,
    jobLabel: string,
    processedDataRows: number,
    expectedDataRows: number,
    status: 'In Progress' | 'Completed' | 'Failed' | 'Pending' | 'Deleted'
    dataFile: string,
    downloadDataFileLink: string,
    downloadStatsFileLink: string,
    dataFileSize: number,
    started: Date,
    lastUpdated: Date,
    message: string,
    jobId: string //hangfire's jobId
}

export interface JobSearchResult{
    resultEntries: JobRecord[],
    offset: number,
    totalMatches: number,
    itemsPerPage: number
}
