<script setup lang="ts">
    import { default as FieldExpression } from './components/FieldExpression.vue'
    import { default as ResultTable } from './components/ResultTable.vue'
    import { useSolrSearchStore } from './store';
    import { computed, ref } from 'vue';
    import { buildQueryString } from './helpers';
    import { eUiMode, SearchFieldDefinition } from './models';

    const props = defineProps<{
        searchFields?: SearchFieldDefinition[],
        queryApi?: string,
        uiMode?: eUiMode
    }>();

    const store = useSolrSearchStore();
    if(props.searchFields){
        store.searchFieldDefinitions = props.searchFields;
    }

    if(props.queryApi){
        store.queryApi = props.queryApi;
    }

    const uiMode = computed(() => props.uiMode ? props.uiMode : eUiMode.Default)


    const expression = computed(() => store.fieldExpression)
    const querySource = computed(() => store.querySource)

    const quertString = computed(() => buildQueryString(store.fieldExpression))
    const rawQuery = ref("")

    const query = () => {
        store.queryResult = null;

        if(uiMode.value === eUiMode.Default){
            store.query(quertString.value, 0, 100)
            store.querySource = "Default Query Result"
        }
        else if(uiMode.value === eUiMode.Raw){
            store.query(rawQuery.value, 0, 100)
            store.querySource = "Raw Query Result"
        }
    }


    </script>
<template>
    Solr search
    <div v-if="uiMode === eUiMode.Default">
        <FieldExpression :model="expression"></FieldExpression>
       <!-- <div class="mt-3 alert alert-info">
            {{ store.fieldExpression }}
        </div>-->
        <div class="mt-3 alert alert-success">
            {{ quertString }}
        </div>        
    </div>
    <div v-if="uiMode === eUiMode.Raw">
        <textarea v-model="rawQuery" class="col-12"></textarea>
    </div>
   
    <button @click="query">Query</button>

    <div class="mt-3 mb-3" v-if="store.queryResult">
       <div class="mt-3">
            <h3>{{ querySource }}</h3>
            Query time: {{store.queryTime}} seconds<br /><br />
            <ResultTable :model="store.queryResult" />
        </div>
    </div>
</template>

