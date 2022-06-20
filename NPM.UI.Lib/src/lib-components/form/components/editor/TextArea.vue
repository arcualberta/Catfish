<script lang="ts">
    import { defineComponent, PropType } from "vue"
    import { useFormSubmissionStore } from '../../store/FormSubmissionStore'
    import * as models from '../../models'
     import MultivalueText from "./MultivalueText.vue"
    export default defineComponent({
        name: "TextArea",
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
  
    <div  v-for="(val, index) in model.values.$values" :key="val.id" class="multilingualField">

        <MultivalueText :model="val" :isMultiline="true" />
        <span v-if="index > 0" class="fa remove-circle" @click="formStore.removeMutilingualValue(model, index)"> x </span>
    </div>
</template>
