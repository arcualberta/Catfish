import { buildHashFromArray } from "@fullcalendar/core";
import { Guid } from "guid-typescript";
import { defineStore } from "pinia";
import { JobRecord, JobSearchResult } from "../models";
import { api } from "@arc/arc-foundation"
import { prefix } from "@fortawesome/free-solid-svg-icons";

export const useJobTrackerStore = defineStore('JobTrackerStore', {
    state: () => ({
        jobSearchResult: {} as JobSearchResult,
        apiRoot: '',
        searchTerm: "",
        isLoadig: false,
        isLoadingFailed: false,
        apiToken: null as string | null,
        tenantId: null as Guid | null
    }),
    actions: {
        async load( offset: number, pageSize: number, isRefreshCall: boolean){

            this.isLoadig = true;

            if(!this.apiToken){
                return
            }

            //update max
            //console.log("searchTerm: " + this.searchTerm)
            //console.log("apiRoot: " + this.apiRoot)
            //console.log("tenantId: " + this.tenantId)
            //console.log("apiToken: " + this.apiToken)
            
            const operation = 2; //Solr Read
            const proxy = new api.SolrProxy(this.apiRoot, this.tenantId as Guid, this.apiToken)
            const data = await proxy.getJobs(offset, pageSize, operation, false, this.searchTerm);
            this.jobSearchResult = data as unknown as JobSearchResult;
            this.isLoadig = false;

            //console.log(JSON.stringify(this.jobSearchResult))
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
        async removeJob(jobId: Guid) {
            const proxy = new api.SolrProxy(this.apiRoot, this.tenantId as Guid, this.apiToken!)
            await proxy.deleteJob(jobId)
        },
        async downloadFile(fileName: string){
            const proxy = new api.SolrProxy(this.apiRoot, this.tenantId as Guid, this.apiToken!)
            await proxy.downloadDataFile(fileName)
        }
    },
    getters:{
        activeJobs: (state) => state.jobSearchResult?.resultEntries?.filter(job => job.status == "In Progress")
    }
});
