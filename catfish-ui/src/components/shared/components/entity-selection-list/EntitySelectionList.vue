<script setup lang="ts">
      import { Guid } from 'guid-typescript';
      import {computed} from 'vue'
      import { Pinia, storeToRefs } from 'pinia' 
      import { useEntitySelectStore } from './store';
      import { EntityData, EntitySearchResult, EntityEntry } from "@/components/entity-editor/models";
import { eEntityType, eSearchTarget } from '../../constants';

       const props = defineProps<{
        piniaInstance: Pinia
    }>();
    //const tableId = props.table.id // Just as an example
        const storeId =Guid.create();
       const entityListStore = useEntitySelectStore(props.piniaInstance);
       entityListStore.seach(eEntityType.Item, eSearchTarget.Title, "title");
       const {entitySearchResult} = storeToRefs(entityListStore);
       //const entityEntries= computed(()=> entitySearchResult.value.result)

       console.log(JSON.stringify(entitySearchResult))
</script>

<template>
    Entity Selection List
    <div v-for="entry in entitySearchResult.result" v-bind:key="entry.id">
        <div>{{entry.title}}</div>
        <div>{{entry.description}}</div>
    </div>
</template>