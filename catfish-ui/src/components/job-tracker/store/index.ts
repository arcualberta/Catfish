import { buildHashFromArray } from "@fullcalendar/core";
import { defineStore } from "pinia";
import { JobRecord, JobSearchResult } from "../models";

export const useJobTrackerStore = defineStore('JobTrackerStore', {
    state: () => ({
        jobSearchResult: {} as JobSearchResult,
        apiRoot: '',
        searchTerm: ""
    }),
    actions: {
        load( offset: number, pageSize: number){
            //update max
            console.log("searchTerm: " + this.searchTerm)
            const api = `${this.apiRoot}/background-job?offset=${offset}&max=${pageSize}&searchTerm=${this.searchTerm}`;
            fetch(api, {
                method: 'GET'
            })
            .then(response => response.json())
            .then(data => {
                this.jobSearchResult = data as JobSearchResult;
            })
            .catch((error) => {
                console.error('Load Error:', error);
            });
        },
        next(pageSize: number) {
            console.log("next")
            this.load(this.jobSearchResult.offset + pageSize, pageSize)        
        },
        previous(pageSize: number) {
            console.log("previous")
            const offset = Math.max(0, (this.jobSearchResult.offset - pageSize))
            this.load(offset, pageSize)
        },
        updateSearchTerm(searchText: string) {
            this.searchTerm = searchText;
           
        }
        
    }
});
