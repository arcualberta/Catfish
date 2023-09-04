import { defineStore } from "pinia";
import { JobRecord } from "../models";

export const useJobTrackerStore = defineStore('JobTrackerStore', {
    state: () => ({
        jobs: [] as JobRecord[],
        offset: 0,
        apiRoot: ''
    }),
    actions: {
        load( offset: number, max: number){

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
        }
    }
});
