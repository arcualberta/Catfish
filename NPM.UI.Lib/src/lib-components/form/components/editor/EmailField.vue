<script lang="ts">
    import { defineComponent, PropType } from "vue";
    import { useFormSubmissionStore } from '../../store/FormSubmissionStore'
    import * as models from '../../models'
    import SingleText from './SingleText.vue'
    export default defineComponent({
        name: "EmailField",
        components:{
            SingleText
        },
        props: {
            model: {
                type: null as PropType<models.MonolingualTextField> | null,
                required: true
            },
            //isMultivalued:
            //{
            //    type: Boolean,
            //    required: false,
            //    default: false
            //}
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
    <div v-for="(val, index) in model?.values?.$values" :key="val" class="monoLingualField">
        <SingleText :model="val" :isMultiline="false" fieldType="email" :index="index" :fieldModel="model" />

    </div>
    <!--<span class="fa plus-circle" @click="formStore.appendMonolingualValue(model)"> + </span>-->

   
</template>
