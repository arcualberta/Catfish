

<script setup lang="ts">
import { computed, onMounted, ref, watch } from 'vue';
import { useJobTrackerStore } from './store';

import 'floating-vue/dist/style.css'
import { Guid } from 'guid-typescript';
import { useLoginStore } from '../login/store';
import { pluckProps } from 'bootstrap-vue-3/dist/utils';

const props = defineProps<{
    apiRoot: string,
    pageSize: number,
    user: string | null,
    admins: string[]   
}>();

const store = useJobTrackerStore();

var jobs = computed(() => store.jobSearchResult.resultEntries)

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
    store.load(0, props.pageSize, false);

    let interval = setInterval(() => {
        if(store.activeJobs?.length == 0){
            clearInterval(interval);
        }
        else{
            store.load(0, props.pageSize, true);
        }
        
/*        if (timer === 0) {
            clearInterval(interval)                
        } else {
            timer--
            console.log(timer)
        }  */           
        }, 5000)
})

    const searchTerm = ref(store.searchTerm);
   


    const reLoad = () => {

        console.log("call reLoad - " + searchTerm.value)
        store.updateSearchTerm(searchTerm.value);
        store.load(0, props.pageSize, false)
    }


    const RemoveJob = (jobId: Guid, index: number, jobLabel: string) => {

        if (confirm('Are you sure you want to delete this job: ' + jobLabel + ' ? ')) {
            // alert('job is deleted');
            store.removeJob(jobId);
            jobs.value.splice(index, 1);
        }
    }

    const shouldAllowDelete = (jobUser:string | null): boolean => 
        (jobUser && jobUser.length > 0 && props.user == jobUser) ||
        (props.user != null && props.admins.includes(props.user))

</script>

<template>
    <div class="mt-2">
        Search Label: <input type="text" v-model="searchTerm" @keyup="reLoad()" /> 
    </div>
    <div class="mt-2" v-if="store.jobSearchResult.resultEntries?.length > 0">
        <span v-if="hasPrev" class="link" @click="store.previous(props.pageSize)">&lt;&lt;&lt;</span>
        {{ first.toLocaleString("en-US") }} to {{ last.toLocaleString("en-US") }} of {{ totalJobs.toLocaleString("en-US") }}
        <span v-if="hasNext" class="link" @click="store.next(props.pageSize)">&gt;&gt;&gt;</span>
    </div>
    <div class="mt-2" style="height:40px;">
        <b-spinner v-if="store.isLoadig" variant="primary" label="Spinning"></b-spinner>
        
        <div v-if="store.isLoadingFailed" class="alert alert-danger">
            Data loading failed!
        </div>
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
                <th scope="col">Actions</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="(job, index) in jobs" :key="job.id.toString()">
                <td>
                    <b-spinner class="status-icon" v-if="job.status == 'In Progress'" variant="success" label="Spinning"></b-spinner>
                    <span>{{ job?.jobId }}</span>
                </td>
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
                <td>
                    <button v-if="shouldAllowDelete(job.user)" @click="RemoveJob(job.id, index, job.jobLabel)" class="btn btn-danger">Delete</button>
                </td>
            </tr>
        </tbody>
    </table>
</template>

<style scoped>
.status-icon{
    width: 20px;
    height: 20px;
}
</style>