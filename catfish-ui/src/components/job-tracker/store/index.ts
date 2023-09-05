import { defineStore } from "pinia";
import { JobRecord } from "../models";

export const useJobTrackerStore = defineStore('JobTrackerStore', {
    state: () => ({
        jobs: [] as JobRecord[],
        offset: 0,
        apiRoot: '',
        max: 100
    }),
    actions: {
        load( offset: number, max: number){
            //update max
            this.max = max;

            const api = `${this.apiRoot}/background-job?offset=${offset}&max=${max}`;
            fetch(api, {
                method: 'GET'
            })
            .then(response => response.json())
            .then(data => {
                    this.jobs = data as JobRecord[];
            })
            .catch((error) => {
                console.error('Load Error:', error);
            });
        },
        next() {
            console.log("next")
            this.load(this.offset + this.max, this.max)
        },
        previous() {
            console.log("previous")
            const offset = this.offset < this.max ? 0 : this.offset - this.max;
            this.load(offset, this.max)
        }   
    }
});
