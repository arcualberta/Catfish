<script lang="ts">
    import { defineComponent, PropType } from 'vue'
   
    import {TextCollection } from '../../models/textModels'
   

    export default defineComponent({
        name: "CheckboxField",
        components: {
            
        },
        props: {
            model: {
                type: Object as PropType<TextCollection> | null,
                required: true
           }

        }
    });
</script>

<template>
    
  
     <div class="checkboxField" v-for="val in model.values" :key="val.id">
           
             <input type="checkbox" id="val.id" name="val.id" value="val.id "/>
             {{val.value}}
             
    </div>
    
</template>

<style scoped>
    .checkboxField{
        margin-right:15px ;
    }
</style>