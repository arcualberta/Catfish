

<script setup lang="ts">
import { computed, onMounted } from 'vue';
import { useJobTrackerStore } from './store';

const props = defineProps<{
    apiRoot: string
}>();

const store = useJobTrackerStore();

const jobs = computed(() => store.jobs)

if(props.apiRoot){
    store.apiRoot = props.apiRoot;
}

onMounted(() => {
    store.load(0, 100);
})



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
            <tr v-for="job in jobs" :key="job.id">
                <td>{{ job.id }}</td>
                <td>{{ job.status }}</td>
                <td>{{ job.jobLabel }}</td>
                <td>{{ job.started }}</td>
                <td>{{ job.lastUpdated }}</td>
                <td>{{ Math.round((job.processedDataRows / job.expectedDataRows) * 10000)/100 }} %</td>
                <td>{{ job.dataFileSize.toLocaleString("en-US") }}</td>
                <td><a :href="job.downloadLink">{{ job.dataFile }}</a></td>
            </tr>
        </tbody>
    </table>
</template>