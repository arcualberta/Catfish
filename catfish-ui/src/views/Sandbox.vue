<script lang="ts" setup>
  import { useEntitySelectStore } from '@/components/shared/components/entity-selection-list/store';
  import { ref } from 'vue'
  import { VueDraggableNext } from 'vue-draggable-next'
  import {default as EntitySelectionList} from '../components/shared/components/entity-selection-list/EntitySelectionList.vue'
  
  const list = ref([
          { name: 'John', id: 1 },
          { name: 'Joao', id: 2 },
          { name: 'Jean', id: 3 },
          { name: 'Gerard', id: 4 },
        ]);

  const log = (event: any) => {
        console.log(event)
      };

  const store1_id = "store-1";
  const store2_id = "store-2";

  const store1 = useEntitySelectStore(store1_id);
  const store2 = useEntitySelectStore(store2_id);

</script>

<template>
  <div class="flex m-10" style="width:50%">
      <draggable class="dragArea list-group w-full" :list="list" @change="log">
          <div class="list-group-item bg-gray-300 m-1 p-3 rounded-md text-center"
                v-for="element in list"
                :key="element.name">
              {{ element.name }}
          </div>
      </draggable>
  </div>

  <h3>List 1</h3>
  <EntitySelectionList :store-id = "store1_id"/>
  <div class="alert alert-info">
    <h3>List 1 Selections</h3>
    {{store1.selectedEntityIds}}
  </div>

  <h3>List 2</h3>
  <EntitySelectionList :store-id = "store2_id"/>

  <div class="alert alert-info">
    <h3>List 2 Selections</h3>
    {{store2.selectedEntityIds}}
  </div>

</template>