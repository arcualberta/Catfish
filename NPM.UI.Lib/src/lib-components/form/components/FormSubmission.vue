<script lang="ts">
    import { defineComponent, PropType, computed } from "vue";


    import { FieldContainer } from '../../shared/entity/formModels'
    //import { useFormStore } from '@/lib-components/form/store/FormStore' //../store/FormStore"
   
    export default defineComponent({
        name: "FormSubmission",
        props: {
            model: {
                type: null as PropType<FieldContainer> | null,
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
