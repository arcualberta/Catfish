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
        },
        setup() {
            const formStore = useFormSubmissionStore();

            return {
                formStore,
            }
        }
        
    });
</script>


<template>
    <div v-for="val in model?.values?.$values" :key="val.id" class="monoLingualField">
        <SingleText :model="val" :is-multiline="false" field-type="email" :field-model="model" />
        <span v-if="model?.values?.$values?.length > 1" class="fa remove-circle" @click="formStore.removeMonolingualValue(model, val.id)"> x </span>
    </div>
</template>
