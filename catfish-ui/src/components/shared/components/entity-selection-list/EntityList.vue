<template>
    <h4>Entity Selection List</h4>
    <div v-for="entry in entitySearchResult?.result" v-bind:key="entry.id.toString()" class="form-control">
        <div>{{entry.title}}  => >{{entry.description}}</div>
    </div>
</template>

<script setup lang="ts">
import {storeToRefs} from 'pinia'
import { useEntitySelectStore } from './store';

 const props = defineProps<{
        storeId: string
           
    }>();
const entityListStore = useEntitySelectStore(props.storeId);
 const {entitySearchResult} = storeToRefs(entityListStore);
</script>
