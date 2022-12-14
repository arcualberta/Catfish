<script setup lang="ts">
    import { default as FieldConstraint } from './components/FieldConstraint.vue'
    import { SolrFields } from './appsettings';
import { useSolrSearchStore } from './store';

    const store = useSolrSearchStore();

    const query = () => store.query();

    </script>
<template>
    Solr search
    <FieldConstraint :searchFields="SolrFields"></FieldConstraint>

    <button @click="query">Query</button>
    <div>
        Query time: {{store.queryTime}} seconds<br /><br />
        {{store.queryResult}}
    </div>
</template>

