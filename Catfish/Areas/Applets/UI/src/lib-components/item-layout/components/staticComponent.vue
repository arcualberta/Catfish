<script lang="ts">
    import { defineComponent, PropType, computed } from 'vue'
    import { StaticText } from '../models/staticText'
   
  
   
    export default defineComponent({
        name: "StaticComponent",
        components: {

        },
        props: {
            model: {
                type: null as PropType<StaticText> | null,
                required: true
            },
        },
        setup(p) {

            return {
                htmlWrapperTag: computed(() => p.model.type?.length > 0 ? p.model.type : "div")
            }
        }
    })

          
        
</script>

<template>

   
    <div class="staticType">
        <h3>Static Field</h3>
     
        <component :is="htmlWrapperTag" :class="model.cssClasses" > {{model.content}} </component>
        
    </div>
</template>

