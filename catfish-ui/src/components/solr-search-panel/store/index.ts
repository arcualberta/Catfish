import { defineStore } from 'pinia';


export const useSolrSearchStore = defineStore('SolrSearchStore', {
    state: () => ({
        queryResult: null as null | object,
        queryStart: 0,
        queryTime: 0
    }),
    actions: {
        query(){
            const api = 'https://localhost:5020/api/solr-search'

            const queryStr = "*:*";
            const form = new FormData();
            form.append("query", queryStr);
            form.append("offset", Math.round(Math.random() * 100).toString())
            form.append("max", "100");

            this.queryStart = new Date().getTime()
            fetch(api, {
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
        },createExpression(){
            
        },
       
    }
});