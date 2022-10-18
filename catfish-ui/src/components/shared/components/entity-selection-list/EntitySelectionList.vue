<script setup lang="ts">
      import { Guid } from 'guid-typescript';
      import {computed} from 'vue'
      import { Pinia, storeToRefs } from 'pinia' 
      import { useEntitySelectStore } from './store';
      import { EntityData, EntitySearchResult, EntityEntry } from "@/components/entity-editor/models";

       const props = defineProps<{
        piniaInstance: Pinia
    }>();
    //const tableId = props.table.id // Just as an example
        const storeId =Guid.create();
       const entityListStore = useEntitySelectStore(props.piniaInstance);
       const {entitySearchResult} = storeToRefs(entityListStore);
       const entityEntries= computed(()=> entitySearchResult.value.result)
</script>

<template>
    Entity Selection List
    <div v-for="entry in entityEntries" v-bind:key="entry.id">
        <div>{{entry.title}}</div>
        <div>{{entry.description}}</div>
    </div>
</template>