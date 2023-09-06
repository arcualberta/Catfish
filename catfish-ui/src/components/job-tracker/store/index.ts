import { buildHashFromArray } from "@fullcalendar/core";
import { defineStore } from "pinia";
import { JobRecord } from "../models";

export const useJobTrackerStore = defineStore('JobTrackerStore', {
    state: () => ({
        jobs: [] as JobRecord[],
        jobsToDisplayPerPage: [] as JobRecord[],
        offset: 0,
        apiRoot: '',
        max: 100
    }),
    actions: {
        load( offset: number, max: number, pageSize: number){
            //update max
            this.max = max;

            const api = `${this.apiRoot}/background-job?offset=${offset}&max=${max}`;
            fetch(api, {
                method: 'GET'
            })
            .then(response => response.json())
            .then(data => {
                this.jobs = data as JobRecord[];
                this.jobsToDisplayPerPage = this.jobs.slice(0, pageSize);
            })
            .catch((error) => {
                console.error('Load Error:', error);
            });
        },
        next(pageSize: number) {
            console.log("next")
            this.offset = this.offset + pageSize;
           
            var toItems = this.offset + pageSize > this.jobs.length ? this.jobs.length : this.offset + pageSize;
            this.jobsToDisplayPerPage = this.jobs.slice(this.offset, toItems);
            
        },
        previous(pageSize: number) {
            console.log("previous")
           
            this.offset =  this.offset - pageSize;
            var toItems = this.offset + pageSize > this.jobs.length ? this.jobs.length : this.offset + pageSize;
            this.jobsToDisplayPerPage = this.jobs.slice(this.offset, toItems);
        }
        
    }
});
