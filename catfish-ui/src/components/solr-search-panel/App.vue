<script setup lang="ts">
    import { default as FieldExpression } from './components/FieldExpression.vue'
    import { default as ResultTable } from './components/ResultTable.vue'
    import { useSolrSearchStore } from './store';
    import { computed, ref, watch } from 'vue';
    import { buildQueryString } from './helpers';
    import { eUiMode, SearchFieldDefinition, SolrEntryType } from './models';
    import { copyToClipboard } from './helpers'

    const props = defineProps<{
        searchFields?: SearchFieldDefinition[],
        resultFieldNames: string[],
        entryTypeFieldName?: string,
        entryTypeFieldOptions?: SolrEntryType[],
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

    watch(() => props.queryApi, async newQueryApi => {
        if(newQueryApi){
            store.queryApi = newQueryApi;
        }
    })

    const selectedEntryType = ref(null as null | SolrEntryType)

    const uiMode = computed(() => props.uiMode ? props.uiMode : eUiMode.Default)

    const expression = computed(() => store.fieldExpression)
    const querySource = computed(() => store.querySource)

    const quertString = computed(() => {
        const q = buildQueryString(store.fieldExpression)
        if(q){
            if(props.entryTypeFieldName && selectedEntryType.value){
                return `(${props.entryTypeFieldName}:${selectedEntryType.value.name}) AND (${q})`;
            }
            else{
                return q;
            }
        }
        else{
            if(props.entryTypeFieldName && selectedEntryType.value){
                return `${props.entryTypeFieldName}:${selectedEntryType.value.name}`;
            }
            else{
                return '*:*';
            }
        }
    })

    const rawQuery = ref("*:*")

    const query = () => {
        store.queryResult = null;

        if(uiMode.value === eUiMode.Default){
            store.query(quertString.value, 0, 100)
            store.querySource = "Filter Result"
        }
        else if(uiMode.value === eUiMode.Raw){
            if(rawQuery.value && rawQuery.value.trim().length > 0){
                store.query(rawQuery.value, 0, 100)
                store.querySource = "Solr Query Result"
            }
            else{
                alert("Please specify a query")
            }
        }
    }

</script>
<template>
    <div v-if="entryTypeFieldName">
        Entry Type:
        <select v-model="selectedEntryType">
            <option value="">ALL</option>
            <option v-for="val in entryTypeFieldOptions" :value="val">{{val.label}}</option>
        </select>
    </div>
    <div v-if="uiMode === eUiMode.Default" class="query-wrapper">
        <FieldExpression :model="expression"></FieldExpression>
    </div>
    <div v-if="uiMode === eUiMode.Default" class="mt-3 query alert alert-success">
        <div>
            <b>Query String</b>
            <font-awesome-icon 
                icon="fa-solid fa-copy" 
                class="fa-icon btn" 
                @click="copyToClipboard(quertString)"
                v-b-tooltip.hover :title="'Copy the query string to clipboard.'" />
        </div>
        <div>{{ quertString }}</div>
    </div>        
    <div v-if="uiMode === eUiMode.Raw">
        <textarea v-model="rawQuery" class="col-12"></textarea>
    </div>

    <div class="mb-3">
        <b>Limit Display Fields</b>
        <div class="row">
            <div v-for="field in store.searchFieldDefinitions" :key="field.name" class="col-md-2 result-field-option">
                <input type="checkbox" :value="field.name" v-model="store.resultFieldNames" /> {{field.label}}
            </div>
        </div>        
    </div>    
    <!--
    <div class="accordion pb-3" role="tablist">
        <b-card no-body class="mb-1">
            <b-card-header header-tag="header" class="p-0 card-header" role="tab">
                <b-button block v-b-toggle.accordion-3 variant="success">
                    Result Columns
                    <font-awesome-icon  icon="fa-chevron-down" class="fa-icon down-arrow" />
                    <font-awesome-icon  icon="fa-chevron-up" class="fa-icon up-arrow" />
                </b-button>
            </b-card-header>
            <b-collapse id="accordion-3" accordion="my-accordion" role="tabpanel">
                <b-card-body>
                <b-card-text>
                    <div class="row">
                        <div v-for="field in store.searchFieldDefinitions" :key="field.name" class="col-md-3 result-field-option">
                            <input type="checkbox" :value="field.name" v-model="store.resultFieldNames" /> {{field.label}}
                        </div>
                    </div>
                </b-card-text>
                </b-card-body>
            </b-collapse>
        </b-card>
    </div>
    -->

     
    <button @click="query" class="btn btn-primary">Search</button>

    <div v-if="store.isLoadig" class="mt-2">
        <b-spinner variant="primary" label="Spinning"></b-spinner>
    </div>

    <div class="mt-3 mb-3" v-if="store.queryResult">
       <div class="mt-3">
            <h4>{{ querySource }}</h4>
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
.accordion .card {
    padding-bottom: 10px;;
}
.card-header{
    margin-top:-30px;
    margin-left:-30px;
    background-color: transparent !important;
    border:none
}
.btn[aria-expanded="false"] .up-arrow{
    display: none;
}
.btn[aria-expanded="true"] .down-arrow{
    display: none;
}
/*
.result-field-option{

}*/
/*
.collapsed > .when-opened,
    :not(.collapsed) > .when-closed {
        display: none;
    }
*/
</style>