<script setup lang="ts">import { computed } from 'vue';
import { SearchResult, SolrFieldData, SolrResultEntry } from '../models';
import { useSolrSearchStore } from '../store';
import { faListDots } from '@fortawesome/free-solid-svg-icons';
import {toTableData, downloadCSV} from '../helpers'

    const props = defineProps<{
        model: SearchResult
    }>();

    const store = useSolrSearchStore()

    const first = computed(() => props.model.offset + 1)
    const last = computed(() => props.model.offset + props.model.resultEntries.length)
    const hasPrev = computed(() => first.value > 1)
    const hasNext = computed(() => last.value < props.model.totalMatches)

    const fieldDefs = computed(()=>store.activeFieldList)
    //const requestedResultFieldNames = computed(()=>store.activeSelectedResultFieldNames)
    const tableData = computed(() => toTableData(props.model.resultEntries, fieldDefs.value, store.activeSelectedResultFieldNames))

    //const downloadData = () => downloadCSV(props.model.resultEntries, fieldDefs.value)
    const getFieldValue=(data: SolrFieldData[], fdkey: string)=>{
       
            const flData = data.find((dt)=>{
                    return dt.key === fdkey
            }); 
          return flData? flData.value: "";
    }
    const downloadCSVData= () => {
    let csv = '';
    //header labels
    fieldDefs.value.forEach(fldef => {
        csv += fldef.label
    });
     //data
    props.model.resultEntries.forEach((row: SolrResultEntry) => {
          
            fieldDefs.value.forEach(fldef => {
                    
                    csv += getFieldValue(row.data, fldef.name) + ","
            });
            csv += "\n";
    });
 
    const anchor = document.createElement('a');
    anchor.href = 'data:text/csv;charset=utf-8,' + encodeURIComponent(csv);
    anchor.target = '_blank';
    anchor.download = 'showtimesSearchResult.csv';
    anchor.click();
}
</script>

<template>
    <div v-if="model.resultEntries.length > 0" class="download-panel">
        <button @click="downloadCSVData()" class="btn btn-success">Download CSV</button>
    </div>
    <div class="mt-2">
        <span v-if="hasPrev" class="link" @click="store.previous()">&lt;&lt;&lt;</span>
        {{ first.toLocaleString("en-US") }} to {{ last.toLocaleString("en-US") }} of {{ model.totalMatches.toLocaleString("en-US") }}
        <span v-if="hasNext" class="link" @click="store.next()">&gt;&gt;&gt;</span></div>
    <hr />

    <b-table hover :items="tableData"></b-table>
</template>

<style scoped>
.link{
    color: #2626ea;
}
.link:hover{
    cursor: pointer;
}
.download-panel{
    float: right;
}
</style>

