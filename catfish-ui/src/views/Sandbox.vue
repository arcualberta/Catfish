
<script lang="ts">
  import { defineComponent } from 'vue'
  import { VueDraggableNext } from 'vue-draggable-next'
  export default defineComponent({
    components: {
      draggable: VueDraggableNext,
    },
    data() {
      return {
        enabled: true,
        list: [
          { name: 'John', id: 1 },
          { name: 'Joao', id: 2 },
          { name: 'Jean', id: 3 },
          { name: 'Gerard', id: 4 },
        ],
        dragging: false,
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
  <h3>Multi-select Box</h3>
  <v-multiselect-listbox :options="['Alabama', 'California']"></v-multiselect-listbox>
<!--  
    <div class="flex m-10" style="width:50%">
        <draggable class="dragArea list-group w-full" :list="list" @change="log">
            <div class="list-group-item bg-gray-300 m-1 p-3 rounded-md text-center"
                 v-for="element in list"
                 :key="element.name">
                {{ element.name }}
            </div>
        </draggable>
    </div>
  -->    
</template>