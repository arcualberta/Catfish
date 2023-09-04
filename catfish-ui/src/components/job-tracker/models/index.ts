import { Guid } from "guid-typescript";

export interface JobRecord{
    id: Guid,
    jobLabel: string,
    processedDataRows: number,
    expectedDataRows: number,
    status: 'In Progress' | 'Completed' | 'Failed',
    dataFile: string,
    downloadLink: string,
    dataFileSize: number,
    started: Date,
    lastUpdated: Date,
    message: string
}
