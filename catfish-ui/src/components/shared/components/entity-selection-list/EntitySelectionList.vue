<script setup lang="ts">
      //import { Guid } from 'guid-typescript';
      //import {computed} from 'vue'
      //import { Pinia, storeToRefs } from 'pinia' 
      import { useEntitySelectStore } from './store';
     //import { EntityData, EntitySearchResult, EntityEntry } from "@/components/entity-editor/models";
    import { eEntityType, eSearchTarget } from '../../constants';
    import {default as EntitySearchBox} from './EntitySearchBox.vue'
    import {default as EntityList} from './EntityList.vue'

       const props = defineProps<{
        storeId: string
    }>();
    
       // const storeId = computed(()=> props.storeId)
      // const entityListStore = useEntitySelectStore(props.piniaInstance);
       const entityListStore = useEntitySelectStore(props.storeId);
      // entityListStore.seach(eEntityType.Item, eSearchTarget.Title, "title");
       //const {entitySearchResult} = storeToRefs(entityListStore);
       
</script>

<template>
     <EntitySearchBox :storeId="storeId" :entityType="eEntityType.Item" />
     <EntityList  :storeId="storeId"/>
</template>