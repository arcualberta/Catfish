<script setup lang="ts">
      import { Guid } from 'guid-typescript';
      import {computed} from 'vue'
      import { Pinia, storeToRefs } from 'pinia' 
      import { useEntitySelectStore } from './store';
     //import { EntityData, EntitySearchResult, EntityEntry } from "@/components/entity-editor/models";
    import { eEntityType, eSearchTarget } from '../../constants';
    import {default as EntitySearchBox} from './EntitySearchBox.vue'
    

       const props = defineProps<{
        storeId: string
    }>();
    
       // const storeId = computed(()=> props.storeId)
      // const entityListStore = useEntitySelectStore(props.piniaInstance);
       const entityListStore = useEntitySelectStore(props.storeId);
      // entityListStore.seach(eEntityType.Item, eSearchTarget.Title, "title");
       const {entitySearchResult} = storeToRefs(entityListStore);
       
</script>

<template>
     <EntitySearchBox :storeId="storeId" :entityType="eEntityType.Item" />
    <h4>Entity Selection List</h4>
    <div v-for="entry in entitySearchResult?.result" v-bind:key="entry.id.toString()">
        <div>{{entry.title}}  => >{{entry.description}}</div>
    </div>
</template>