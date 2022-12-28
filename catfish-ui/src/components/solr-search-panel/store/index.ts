import { defineStore } from 'pinia';
import { buildQueryString } from '../helpers';
import { SearchFieldDefinition, SearchResult } from '../models';
import { ConstraintType, createFieldExpression, FieldExpression } from '../models/FieldExpression';


export const useSolrSearchStore = defineStore('SolrSearchStore', {
    state: () => ({
        fieldExpression: createFieldExpression(),
        querySource: null as string | null,
        activeQueryString: "",
        searchFieldDefinitions: [] as SearchFieldDefinition[],
        resultFieldNames: [] as string[],
        queryResult: null as null | SearchResult,
        offset: 0,
        max: 100,
        queryStart: 0,
        queryTime: 0,
        queryApi: 'https://localhost:5020/api/solr-search',
        isLoadig: false
    }),
    actions: {
        query(query: string | null, offset: number, max: number){
            this.isLoadig = true;
            this.offset = offset;
            this.max = max;

            this.activeQueryString = query && query.trim().length > 0 ? query : "*:*";
            const form = new FormData();
            form.append("query", this.activeQueryString);
            form.append("offset", offset.toString())
            form.append("max", max.toString());

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
    }
});