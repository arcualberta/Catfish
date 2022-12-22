<script setup lang="ts">
    import { default as FieldExpression } from './components/FieldExpression.vue'
    import { useSolrSearchStore } from './store';
    import { computed } from 'vue';
    import { buildQueryString } from './helpers';
    import { SearchFieldDefinition } from './models';

    const props = defineProps<{
        searchFields?: SearchFieldDefinition[],
        queryApi?: string
    }>();

    const store = useSolrSearchStore();
    if(props.searchFields){
        store.searchFieldDefinitions = props.searchFields;
    }

    if(props.queryApi){
        store.queryApi = props.queryApi;
    }

    const expression = computed(() => store.fieldExpression)

    const query = () => store.query(0, 100);

    const quertString = computed(() => buildQueryString(store.fieldExpression))

    </script>
<template>
    Solr search
    <div >
        <FieldExpression :model="expression"></FieldExpression>
    </div>
   
    <div class="mt-3 mb-3">
        <button @click="query">Query</button>
        <div class="mt-3 alert alert-info">
            {{ store.fieldExpression }}
        </div>
        <div class="mt-3 alert alert-success">
            {{ quertString }}
        </div>
        <div class="mt-3">
            Query time: {{store.queryTime}} seconds<br /><br />
            {{store.queryResult}}
        </div>
    </div>
</template>

