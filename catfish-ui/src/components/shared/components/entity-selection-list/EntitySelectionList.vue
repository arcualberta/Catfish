<script setup lang="ts">
      import { Guid } from 'guid-typescript';
      //import {computed} from 'vue'
      import { Pinia, storeToRefs } from 'pinia' 
      import { useEntitySelectStore } from './store';
     //import { EntityData, EntitySearchResult, EntityEntry } from "@/components/entity-editor/models";
    import { eEntityType, eSearchTarget } from '../../constants';

       const props = defineProps<{
        storeId: string
    }>();
    //const tableId = props.table.id // Just as an example
        const storeId =Guid.create();
      // const entityListStore = useEntitySelectStore(props.piniaInstance);
       const entityListStore = useEntitySelectStore(props.storeId);
       entityListStore.seach(eEntityType.Item, eSearchTarget.Title, "title");
       const {entitySearchResult} = storeToRefs(entityListStore);
       
</script>

<template>
    <h4>Entity Selection List</h4>
    <div v-for="entry in entitySearchResult?.result" v-bind:key="entry.id.toString()">
        <div>{{entry.title}}  => >{{entry.description}}</div>
    </div>
</template>