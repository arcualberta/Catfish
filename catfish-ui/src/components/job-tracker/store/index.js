import { defineStore } from "pinia";
import { api } from "@arc/arc-foundation";
export const useJobTrackerStore = defineStore('JobTrackerStore', {
    state: () => ({
        jobSearchResult: {},
        apiRoot: '',
        searchTerm: "",
        isLoadig: false,
        isLoadingFailed: false,
        apiToken: null,
        tenantId: null
    }),
    actions: {
        async load(offset, pageSize, isRefreshCall) {
            this.isLoadig = true;
            if (!this.apiToken) {
                return;
            }
            //update max
            //console.log("searchTerm: " + this.searchTerm)
            //console.log("apiRoot: " + this.apiRoot)
            //console.log("tenantId: " + this.tenantId)
            //console.log("apiToken: " + this.apiToken)
            const operation = 2; //Solr Read
            const proxy = new api.SolrProxy(this.apiRoot, this.tenantId, this.apiToken);
            const data = await proxy.getJobs(offset, pageSize, operation, false, this.searchTerm);
            this.jobSearchResult = data;
            this.isLoadig = false;
            //console.log(JSON.stringify(this.jobSearchResult))
        },
        next(pageSize) {
            console.log("next");
            this.load(this.jobSearchResult.offset + pageSize, pageSize, false);
        },
        previous(pageSize) {
            console.log("previous");
            const offset = Math.max(0, (this.jobSearchResult.offset - pageSize));
            this.load(offset, pageSize, false);
        },
        updateSearchTerm(searchText) {
            this.searchTerm = searchText;
        },
        async removeJob(jobId) {
            const proxy = new api.SolrProxy(this.apiRoot, this.tenantId, this.apiToken);
            await proxy.deleteJob(jobId);
        },
        async downloadFile(fileName) {
            const proxy = new api.SolrProxy(this.apiRoot, this.tenantId, this.apiToken);
            await proxy.downloadDataFile(fileName);
        }
    },
    getters: {
        activeJobs: (state) => state.jobSearchResult?.resultEntries?.filter(job => job.status == "In Progress")
    }
});
//# sourceMappingURL=index.js.map