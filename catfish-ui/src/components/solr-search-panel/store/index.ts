import { defineStore } from 'pinia';
import { buildQueryString } from '../helpers';
import { DataSourceOption, SearchFieldDefinition, SearchResult, SolrEntryType } from '../models';
import { ConstraintType, createFieldExpression, FieldExpression } from '../models/FieldExpression';


export const useSolrSearchStore = defineStore('SolrSearchStore', {
    state: () => ({
        fieldExpression: createFieldExpression(),
        querySource: null as string | null,
        activeQueryString: "",
        searchFieldDefinitions: [] as SearchFieldDefinition[],
        resultFieldNames: [] as string[],
        selectedEntryType: null as SolrEntryType | null,
        selectedDataSource: null as DataSourceOption | null,
        selectedEntryTypeBackup: null as SolrEntryType | null,
        queryResult: null as null | SearchResult,
        offset: 0,
        max: 100,
        queryStart: 0,
        queryTime: 0,
        queryApi: 'https://localhost:5020/api/solr-search',
        isLoadig: false,
        isLoadingFailed: false,
        jobId:"",
        user: null as string | null
    }),
    actions: {
        query(query: string | null, offset: number, max: number){
            this.isLoadig = true;
            this.isLoadingFailed = false;
            this.offset = offset;
            this.max = max;

            this.activeQueryString = query && query.trim().length > 0 ? query : "*:*";

            const form = new FormData();
            form.append("query", this.activeQueryString);
            form.append("offset", offset.toString())
            form.append("max", max.toString());
            if(this.resultFieldNames.length > 0){
                form.append("fieldList", "id,"+this.resultFieldNames.join());
            }
            
            this.queryStart = new Date().getTime()
            fetch(this.queryApi, {
                method: 'POST',
                body: form,
                headers: {
                        'encType': 'multipart/form-data'
                },
            })
            .then(response => response.json())
            .then(data => {
                    this.queryResult = data;
                    this.queryTime = (new Date().getTime()- this.queryStart)/1000.0
                    this.isLoadig = false;
            })
            .catch((error) => {
                console.error('Load Entities API Error:', error);
                this.isLoadig = false;
                this.isLoadingFailed = true;
            });
        },
        executeJob(query: string | null, email: string, label: string, batchSize:number, selectUniqueEntries:boolean, roundFloats:boolean, numDecimalPoints:number, frequencyArrayFields: string[], uniqueExportFields: string[]) {
            this.isLoadig = true;
           // this.offset = offset;
           // this.max = max;

            this.activeQueryString = query && query.trim().length > 0 ? query : "*:*";

            const form = new FormData();
            form.append("query", this.activeQueryString);
            form.append("email", email)
            form.append("label", label);
            form.append("batchSize", batchSize.toString());
            if(this.user?.length && this.user?.length> 0){
                form.append("user", this.user)
            }

            if(this.resultFieldNames?.length > 0){
                form.append("fieldList", this.resultFieldNames.join());
            }

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

            var querySearchJobApi = this.queryApi + "/schedule-search-job"
            console.log("API: ", querySearchJobApi)

            fetch(querySearchJobApi, {
                method: 'POST',
                body: form,
                headers: {
                    'encType': 'multipart/form-data'
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
        },
        next(){
            console.log("next")
            this.query(this.activeQueryString, this.offset+this.max, this.max)
        },
        previous(){
            console.log("previous")
            const offset = this.offset < this.max ? 0 : this.offset - this.max;
            this.query(this.activeQueryString, offset, this.max)
        }   
    },
    getters: {
        activeFieldList: (state) => {
            if(state.selectedEntryType){
                const selected = state.searchFieldDefinitions?.filter(fd => Array.isArray(fd.entryType) 
                    ? (fd.entryType as number[]).includes(state.selectedEntryType!.entryType) 
                    : (fd.entryType as number) === state.selectedEntryType!.entryType);

                return selected.sort((a, b) => a.label.toLowerCase() > b.label.toLowerCase() ? 1 : -1);
            }
            else{
                return state.searchFieldDefinitions.sort((a, b) => a.label.toLowerCase() > b.label.toLowerCase() ? 1 : -1);
            }            
        },
        activeSelectedResultFieldNames: (state) => {
            return state.resultFieldNames.filter(fieldName => state.activeFieldList.filter(fd => fd.name == fieldName)?.length > 0)
        },
        resultArrayFields: (state) => state.resultFieldNames.filter(fieldName => fieldName.match(new RegExp('.*_.+s$')))
        
    }
});