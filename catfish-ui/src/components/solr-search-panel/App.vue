<script setup lang="ts">
    import { default as FieldExpression } from './components/FieldExpression.vue'
import { useSolrSearchStore } from './store';
import { computed } from 'vue';

    const store = useSolrSearchStore();
    const expression = computed(() => store.fieldExpression)

    const query = () => store.query();

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
        <div class="mt-3">
            Query time: {{store.queryTime}} seconds<br /><br />
            {{store.queryResult}}
        </div>
    </div>
</template>

