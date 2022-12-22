import { defineStore } from 'pinia';
import { buildQueryString } from '../helpers';
import { SearchFieldDefinition } from '../models';
import { ConstraintType, createFieldExpression, FieldExpression } from '../models/FieldExpression';


export const useSolrSearchStore = defineStore('SolrSearchStore', {
    state: () => ({
        fieldExpression: createFieldExpression(),
        searchFieldDefinitions: [] as SearchFieldDefinition[],
        queryResult: null as null | object,
        queryStart: 0,
        queryTime: 0,
        queryApi: 'https://localhost:5020/api/solr-search'
    }),
    actions: {
        query(offset: number, max: number){
            const userQueryStr = buildQueryString(this.fieldExpression);
            const queryStr = userQueryStr ? userQueryStr : "*:*";
            const form = new FormData();
            form.append("query", queryStr);
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
            })
            .catch((error) => {
                console.error('Load Entities API Error:', error);
            });
        }     
    }
});