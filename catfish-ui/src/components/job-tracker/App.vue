

<script setup lang="ts">
import { computed, onMounted, ref, watch } from 'vue';
import { useJobTrackerStore } from './store';

import 'floating-vue/dist/style.css'

const props = defineProps<{
    apiRoot: string,
    pageSize: number,
   
}>();

const store = useJobTrackerStore();

    const jobs = computed(() => store.jobSearchResult.resultEntries)
    
    console.log("total matched" + store.jobSearchResult.totalMatches ? store.jobSearchResult.totalMatches: 0)
    var totalJobs = computed(() => store.jobSearchResult.totalMatches ? store.jobSearchResult.totalMatches: 0);
    const first = computed(() => store.jobSearchResult.offset + 1)
    const last = computed(() => store.jobSearchResult.offset + store.jobSearchResult.resultEntries?.length) 
    const hasPrev = computed(() => first.value > 1)
    const hasNext = computed(() => (last.value < store.jobSearchResult.totalMatches))

   
if(props.apiRoot){
    store.apiRoot = props.apiRoot;
}

    onMounted(() => {
        
        store.load(0, props.pageSize);
})

    const searchTerm = ref(store.searchTerm);
   


    const reLoad = () => {

        console.log("call reLoad - " + searchTerm.value)
        store.updateSearchTerm(searchTerm.value);
        store.load(0, props.pageSize)
    }

</script>

id: Guid,
    jobLabel: string,
    processedDataRows: number,
    expectedDataRows: number,
    status: 'In Progress' | 'Completed' | 'Failed',
    dataFile: string,
    downloadLink: string,
    dataFileSize: number,
    started: Date,
    lastUpdated: Date
<template>
    <div class="mt-2">
       
        Search Label: <input type="text" v-model="searchTerm" @keyup.enter="reLoad()" /> 
    </div>
    <div class="mt-2">
        <span v-if="hasPrev" class="link" @click="store.previous(props.pageSize)">&lt;&lt;&lt;</span>
        {{ first.toLocaleString("en-US") }} to {{ last.toLocaleString("en-US") }} of {{ totalJobs.toLocaleString("en-US") }}
        <span v-if="hasNext" class="link" @click="store.next(props.pageSize)">&gt;&gt;&gt;</span>
    </div>
    <div v-if="store.isLoadig" class="mt-2">
        <b-spinner variant="primary" label="Spinning"></b-spinner>
    </div>
    <div v-if="store.isLoadingFailed" class="alert alert-danger">
        Data loading failed!
    </div>
    <table class="table">
        <thead>
            <tr>
                <th scope="col">Id</th>
                <th scope="col">Status</th>
                <th scope="col">Label</th>
                <th scope="col">Started</th>
                <th scope="col">Last Updated</th>
                <th scope="col">Progress</th>
                <th scope="col">File Size</th>
                <th scope="col">Data File</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="job in jobs" :key="job.id.toString()">
                <td>{{ job.id }}</td>
                <td>
                    <span v-tooltip="job.message">{{ job.status }}</span>
                </td>
                <td>{{ job.jobLabel }}</td>
                <td>{{ job.started }}</td>
                <td>{{ job.lastUpdated }}</td>
                <td>{{ Math.round((job.processedDataRows / job.expectedDataRows) * 10000)/100 }} %</td>
                <td>{{ job.dataFileSize.toLocaleString("en-US") }}</td>
                <td>
                    <div><a :href="job.downloadDataFileLink">{{ job.dataFile }}</a></div>
                    <div v-if="job.downloadStatsFileLink"><a :href="job.downloadStatsFileLink">stats.csv</a></div>
                </td>
            </tr>
        </tbody>
    </table>
</template>