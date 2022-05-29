<script lang="ts">
    import { defineComponent, PropType, computed } from "vue";

    import * as models from '../models'
   
    export default defineComponent({
        name: "FormSubmission",
        props: {
            model: {
                type: null as PropType<models.FieldContainer> | null,
                required: false
            },
         },
        setup(p) {

           return{
               form: computed(() => p.model)
           }
        },
    });
</script>

<template>
    <h3>Form</h3>
    {{JSON.stringify(model)}}
</template>
