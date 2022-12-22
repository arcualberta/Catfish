<script setup lang="ts">import { computed } from 'vue';
import { SearchResult, SolrFieldData, SolrResultEntry } from '../models';
import { useSolrSearchStore } from '../store';
import { faListDots } from '@fortawesome/free-solid-svg-icons';

    const props = defineProps<{
        model: SearchResult
    }>();

    const store = useSolrSearchStore()

    const first = computed(() => props.model.offset + 1)
    const last = computed(() => props.model.offset + props.model.resultEntries.length)
    const hasPrev = computed(() => first.value > 1)
    const hasNext = computed(() => last.value < props.model.totalMatches)

    const fieldDefs = computed(()=>store.searchFieldDefinitions)

    const getFieldValue=(data: SolrFieldData[], fdkey: string)=>{
       
            const flData = data.find((dt)=>{
                    return dt.key === fdkey
            }); 
          return flData? flData.value.toString(): "";
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
   <div><button v-if="model.resultEntries.length > 0" @click="downloadCSVData">Download CSV</button></div>
    <div class="mt-2">
        <span v-if="hasPrev" class="link" @click="store.previous()">&lt;&lt;&lt;</span>
        {{ first }} to {{ last }} of {{ model.totalMatches }}
        <span v-if="hasNext" class="link" @click="store.next()">&gt;&gt;&gt;</span></div>
    <hr />
    <table>
        <thead>
        <tr>
          <th v-for="fh in fieldDefs" :key="fh.name">{{fh.label}}</th>
        </tr>
        </thead>
        <tbody>
            <tr v-for="row in model.resultEntries">
                
                <td v-for="fh in fieldDefs" :key="fh.name" :class="fh.name==='synopsis_t' || fh.name === 'casts_ts'?'truncate':''">
                    
                   {{getFieldValue(row.data, fh.name)}}
                </td>
            </tr>
            </tbody>
    </table>
</template>

<style scoped>
.link{
    color: #2626ea;
}
.link:hover{
    cursor: pointer;
}
</style>

