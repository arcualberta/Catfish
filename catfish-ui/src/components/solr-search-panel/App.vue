<script setup lang="ts">
    import { default as FieldExpression } from './components/FieldExpression.vue'
    import { default as ResultTable } from './components/ResultTable.vue'
    import { default as TransientMessage } from '../shared/components/transient-message/TransientMessage.vue'
    import { useSolrSearchStore } from './store';
    import { computed, ref } from 'vue';
    import { buildQueryString } from './helpers';
    import { eUiMode, SearchFieldDefinition } from './models';
    import { copyToClipboard } from './helpers'
    import { Guid } from 'guid-typescript';

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

    const quertString = computed(() => {
        const q = buildQueryString(store.fieldExpression)
        return q ? q : "*:*"
    })

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

    const copyButtonId = "copy" + Guid.create() as unknown as string
    const toggleIconClass = () => {
        document.getElementById(copyButtonId)?.toggleAttribute()
    }

</script>
<template>
    
    <div v-if="uiMode === eUiMode.Default" class="query-wrapper">
        <FieldExpression :model="expression"></FieldExpression>
        <div class="mt-3 alert alert-success">
            <div>
                <b>Query String</b>
                <font-awesome-icon 
                    :id = "copyButtonId"
                    icon="fa-solid fa-copy" 
                    class="fa-icon btn" 
                    @click="copyToClipboard(quertString); toggleIconClass()"
                    v-b-tooltip.hover :title="'Copy the query string to clipboard.'" />
            </div>
            {{ quertString }}
        </div>        
    </div>
    <div v-if="uiMode === eUiMode.Raw">
        <textarea v-model="rawQuery" class="col-12"></textarea>
    </div>
    <button @click="query" class="btn btn-primary">Search</button>

    <div class="mt-3 mb-3" v-if="store.queryResult">
       <div class="mt-3">
            <h3>{{ querySource }}</h3>
            Query time: {{store.queryTime}} seconds<br /><br />
            <ResultTable :model="store.queryResult" />
        </div>
    </div>
</template>

<style scoped>
.query-wrapper{
    margin-left: -15px;
}
.fa-icon.btn{
    width:15px;
    height:15px;
    padding: 3px;
}
.copy-link:hover {
    cursor: pointer;
}
</style>