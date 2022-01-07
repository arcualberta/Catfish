<script lang="ts">
    import { defineComponent, PropType } from 'vue'
    import { InfoSection } from '../../models/fieldContainer'

    export default defineComponent({
        name: "InfoField",
        props: {
            model: {
                type: null as PropType<InfoSection> | null,
                required: true
           }
        }
    });
</script>

<template>
  
   <div v-for="val in model.content.values" :key="val.id">
       <div v-html="val.value"></div>
   </div>
    
</template>

