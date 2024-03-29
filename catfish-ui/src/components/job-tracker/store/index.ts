import { buildHashFromArray } from "@fullcalendar/core";
import { Guid } from "guid-typescript";
import { defineStore } from "pinia";
import { JobRecord, JobSearchResult } from "../models";

export const useJobTrackerStore = defineStore('JobTrackerStore', {
    state: () => ({
        jobSearchResult: {} as JobSearchResult,
        apiRoot: '',
        searchTerm: "",
        isLoadig: false,
        isLoadingFailed: false
    }),
    actions: {
        load( offset: number, pageSize: number, isRefreshCall: boolean){
            //update max
            console.log("searchTerm: " + this.searchTerm)
            const api = `${this.apiRoot}/background-job?offset=${offset}&max=${pageSize}&searchTerm=${this.searchTerm}&isRefreshCall=${isRefreshCall}`;
            this.isLoadig = true;
            this.isLoadingFailed = false;
            fetch(api, {
                method: 'GET'
            })
            .then(response => response.json())
            .then(data => {
                if(isRefreshCall) {
                    (data as JobSearchResult).resultEntries.forEach(job => {
                        const target = this.jobSearchResult.resultEntries.find(child => child.id == job.id)
                        if(target) {
                            target.status = job.status;
                            target.lastUpdated = job.lastUpdated;
                            target.processedDataRows = job.processedDataRows;
                            target.dataFileSize = job.dataFileSize;                            
                        }
                    });
                }
                else{
                    this.jobSearchResult = data as JobSearchResult;
                }
                this.isLoadig = false;
            })
            .catch((error) => {
                console.error('Load Error:', error);
                this.isLoadingFailed = true;
                this.isLoadig = false;

            });
        },
        next(pageSize: number) {
            console.log("next")
            this.load(this.jobSearchResult.offset + pageSize, pageSize, false)        
        },
        previous(pageSize: number) {
            console.log("previous")
            const offset = Math.max(0, (this.jobSearchResult.offset - pageSize))
            this.load(offset, pageSize, false)
        },
        updateSearchTerm(searchText: string) {
            this.searchTerm = searchText;
           
        },
        removeJob(jobId: Guid) {
            //const form = new FormData();
            //form.append("jobId", jobId.toString());
            
            var removeJobApi = `${this.apiRoot}/background-job/remove-job?jobId=${jobId}`;
            fetch(removeJobApi, {
                method: 'POST',
               // body: form,
                headers: {
                    'encType': 'multipart/form-data'
                },
            })
                .then(response => response.json())
                .then(data => {
                    alert("Job has been canceled.")
                })
                .catch((error) => {
                    console.error('Error on trying canceling the job', error);
                  
                });
        }
    },
    getters:{
        activeJobs: (state) => state.jobSearchResult?.resultEntries?.filter(job => job.status == "In Progress")
    }
});
