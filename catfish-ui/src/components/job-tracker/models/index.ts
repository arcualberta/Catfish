import { Guid } from "guid-typescript";

export interface JobRecord{
    id: Guid,
    jobLabel: string,
    processedDataRows: number,
    expectedDataRows: number,
    status: 'In Progress' | 'Completed' | 'Failed' | 'Pending'
    dataFile: string,
    downloadDataFileLink: string,
    downloadStatsFileLink: string,
    dataFileSize: number,
    started: Date,
    lastUpdated: Date,
    message: string,
    jobId: string, //hangfire's jobId
    isDeleted: boolean | null,
    deletedDate: Date | null,
    user: string | null
}

export interface JobSearchResult{
    resultEntries: JobRecord[],
    offset: number,
    totalMatches: number,
    itemsPerPage: number
}
