<script lang="ts">
    import { boolean } from 'joi';
import { defineComponent, PropType } from 'vue'
    import { MultilingualTextInput } from '../../models/fieldContainer'


    export default defineComponent({
        name: "TextField",
        props: {
            model: {
                type: null as PropType<MultilingualTextInput> | null,
                required: true
           },
            isMultivalued: 
            {
                type: boolean,
                required: false,
                default: true
            },
            isMultiline: {
                type: boolean,
                required: false,
                default: true
            }

        }
    });
</script>

<template>
   <div v-for="val in Model.values" :key="val.id">
       <div>{{val.concatenatedContent}}</div>
   </div>
    
</template>

