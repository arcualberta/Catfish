
<script lang="ts">
  import { defineComponent } from 'vue'
  import { VueDraggableNext } from 'vue-draggable-next'
  import {default as EntitySelectionList} from '../components/shared/components/entity-selection-list/EntitySelectionList.vue'
  import { createPinia } from 'pinia'
  export default defineComponent({
    components: {
          draggable: VueDraggableNext,
          EntitySelectionList
          
    },
    data() {
       
      const piniaInstance = createPinia();

      return {
        enabled: true,
        list: [
          { name: 'John', id: 1 },
          { name: 'Joao', id: 2 },
          { name: 'Jean', id: 3 },
          { name: 'Gerard', id: 4 },
        ],
        dragging: false,
        piniaInstance
      }
    },
    methods: {
      log(event: any) {
        console.log(event)
      },
    },
  })
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

  <EntitySelectionList :piniaInstance = "piniaInstance"/>
</template>