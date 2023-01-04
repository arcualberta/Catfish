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

    const downloadData = () => downloadCSV(props.model.resultEntries, fieldDefs.value)

</script>

<template>
    <div v-if="model.resultEntries.length > 0" class="download-panel">
        <button @click="downloadData()" class="btn btn-danger">Download CSV</button>
    </div>
    <div class="mt-2">
        <span v-if="hasPrev" class="link" @click="store.previous()">&lt;&lt;&lt;</span>
        {{ first }} to {{ last }} of {{ model.totalMatches }}
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

