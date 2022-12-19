<script setup lang="ts">
    import { default as FieldExpression } from './components/FieldExpression.vue'
    import { SolrFields } from './appsettings';
import { useSolrSearchStore } from './store';

    const store = useSolrSearchStore();

    const query = () => store.query();

    </script>
<template>
    Solr search
    <div style="border-style: solid;border-color: green;">
        <FieldExpression :searchFields="SolrFields" :value="[]"></FieldExpression>
    </div>
   
    <button @click="query">Query</button>
    <div>
        Query time: {{store.queryTime}} seconds<br /><br />
        {{store.queryResult}}
    </div>
</template>

