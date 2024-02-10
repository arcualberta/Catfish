import { defineStore } from 'pinia';
import { createFieldExpression } from '../models/FieldExpression';
import { api } from '@arc/arc-foundation';
export const useSolrSearchStore = defineStore('SolrSearchStore', {
    state: () => ({
        fieldExpression: createFieldExpression(),
        querySource: null,
        activeQueryString: "",
        searchFieldDefinitions: [],
        resultFieldNames: [],
        selectedEntryType: null,
        selectedDataSource: null,
        selectedEntryTypeBackup: null,
        queryResult: null,
        offset: 0,
        max: 100,
        queryStart: 0,
        queryTime: 0,
        queryApi: null,
        isLoadig: false,
        isLoadingFailed: false,
        jobId: "",
        user: null,
        apiToken: null,
        tenantId: null
    }),
    actions: {
        async query(query, offset, max) {
            this.isLoadig = true;
            this.isLoadingFailed = false;
            this.activeQueryString = query && query.trim().length > 0 ? query : "*:*";
            this.offset = offset;
            this.max = max;
            const params = {
                query: this.activeQueryString,
                offset: this.offset,
                max: this.max
            };
            if (this.resultFieldNames.length > 0) {
                params.fieldList = "id," + this.resultFieldNames.join();
            }
            this.queryStart = new Date().getTime();
            try {
                const proxy = new api.SolrProxy(this.selectedDataSourceQueryApi, this.tenantId, this.apiToken);
                const data = await proxy.search(params);
                this.queryResult = data;
                this.queryTime = (new Date().getTime() - this.queryStart) / 1000.0;
                this.isLoadig = false;
            }
            catch (error) {
                console.error('Load Entities API Error:', error);
                this.isLoadig = false;
                this.isLoadingFailed = true;
            }
        },
        async executeJob(query, email, label, batchSize, selectUniqueEntries, roundFloats, numDecimalPoints, frequencyArrayFields, uniqueExportFields) {
            this.isLoadig = true;
            // this.offset = offset;
            // this.max = max;
            this.activeQueryString = query && query.trim().length > 0 ? query : "*:*";
            const params = {};
            params.query = this.activeQueryString;
            params.jobLabel = label;
            params.batchSize = batchSize;
            if (this.user?.length && this.user?.length > 0) {
                params.user = this.user;
            }
            if (this.resultFieldNames?.length > 0) {
                params.fieldList = this.resultFieldNames.join();
            }
            if (selectUniqueEntries) {
                params.selectUniqueEntries = true;
                if (roundFloats) {
                    params.numDecimalPoints = numDecimalPoints;
                }
            }
            if (frequencyArrayFields?.length > 0) {
                params.frequencyArrayFields = frequencyArrayFields.join();
            }
            if (uniqueExportFields?.length > 0) {
                params.exportFields = uniqueExportFields.join();
            }
            this.queryStart = new Date().getTime();
            const proxy = new api.SolrProxy(this.selectedDataSourceQueryApi, this.tenantId, this.apiToken);
            const data = await proxy.scheduleSearchJob(params);
            this.jobId = data;
            this.isLoadig = false;
            alert("Jod has been successfully submitted: job id " + this.jobId);
            /*
                       const form = new FormData();
                       //form.append("query", this.activeQueryString);
                       //form.append("email", email)
                       //form.append("label", label);
                       //form.append("batchSize", batchSize.toString());
                       //if(this.user?.length && this.user?.length> 0){
                       //    form.append("user", this.user)
                       //}
           
                       //if(this.resultFieldNames?.length > 0){
                       //    form.append("fieldList", this.resultFieldNames.join());
                       //}
           
                       if(selectUniqueEntries){
                           form.append("selectUniqueEntries", selectUniqueEntries.toString());
                           if(roundFloats){
                               form.append("numDecimalPoints", numDecimalPoints.toString());
                           }
                       }
           
                       if(frequencyArrayFields?.length > 0){
                           form.append("frequencyArrayFields", frequencyArrayFields.join());
                       }
           
                       if(uniqueExportFields?.length > 0){
                           form.append("exportFields", uniqueExportFields.join());
                       }
           
                       //console.log("uniqueExportFields", JSON.stringify(uniqueExportFields))
                       
                       this.queryStart = new Date().getTime()
           
                       var querySearchJobApi = this.selectedDataSourceQueryApi + "/schedule-search-job"
                       console.log("API: ", querySearchJobApi)
           
           
           
                       fetch(querySearchJobApi, {
                           method: 'POST',
                           body: form,
                           headers: {
                               'encType': 'multipart/form-data',
                               'Authorization': `bearer ${this.apiToken}`,
                               'Tenant-Id': `${this.tenantId}`
                           },
                       })
                           .then(response => response.json())
                           .then(data => {
                               this.jobId = data;
                               this.isLoadig = false;
                               alert("Jod has been successfully submitted: job id " + this.jobId);
                           })
                           .catch((error) => {
                               console.error('Load Entities API Error:', error);
                               this.isLoadig = false;
                           });
           
                           */
        },
        next() {
            console.log("next");
            this.query(this.activeQueryString, this.offset + this.max, this.max);
        },
        previous() {
            console.log("previous");
            const offset = this.offset < this.max ? 0 : this.offset - this.max;
            this.query(this.activeQueryString, offset, this.max);
        }
    },
    getters: {
        selectedDataSourceQueryApi: (state) => {
            if (state.selectedDataSource && state.selectedDataSource.api?.length > 0) {
                return state.selectedDataSource.api;
            }
            else {
                return state.queryApi;
            }
        },
        getSelectedDataSource: (state) => {
            return state.selectedDataSource;
        },
        activeFieldList: (state) => {
            if (state.selectedEntryType) {
                const selected = state.searchFieldDefinitions?.filter(fd => Array.isArray(fd.entryType)
                    ? fd.entryType.includes(state.selectedEntryType.entryType)
                    : fd.entryType === state.selectedEntryType.entryType);
                return selected.sort((a, b) => a.label.toLowerCase() > b.label.toLowerCase() ? 1 : -1);
            }
            else {
                return state.searchFieldDefinitions.sort((a, b) => a.label.toLowerCase() > b.label.toLowerCase() ? 1 : -1);
            }
        },
        activeSelectedResultFieldNames: (state) => {
            return state.resultFieldNames.filter(fieldName => state.activeFieldList.filter(fd => fd.name == fieldName)?.length > 0);
        },
        resultArrayFields: (state) => state.resultFieldNames.filter(fieldName => fieldName.match(new RegExp('.*_.+s$')))
    }
});
//# sourceMappingURL=index.js.map