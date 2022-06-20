<script lang="ts">
    import { defineComponent, PropType } from "vue"
    import { useFormSubmissionStore } from '../../store/FormSubmissionStore'
    import * as models from '../../models'
    import MultivalueText from "./MultivalueText.vue"

    export default defineComponent({
        name: "TextField",
        components:{
           MultivalueText
        },
        props: {
            model: {
                type: null as PropType<models.MultilingualTextField> | null, //PropType<models.TextField>
                required: false
            },
        },
        setup() {
            const formStore = useFormSubmissionStore();

            return {
                formStore
            }
        }
        
    });
</script>


<template>
   
    <div  v-for="(val in model.values.$values" :key="val.id" class="multilingualField">
        <MultivalueText :model="val" :isMultiline="false" />
        <span v-if="model.values.$values?.length > 1" class="fa remove-circle" @click="formStore.removeMutilingualValue(model, val.id)"> x </span>
    </div>
  
</template>
