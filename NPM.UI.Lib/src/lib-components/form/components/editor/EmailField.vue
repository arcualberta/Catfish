<script lang="ts">
    import { defineComponent, PropType } from "vue";
    import { useFormSubmissionStore } from '../../store/FormSubmissionStore'
    import * as models from '../../models'
    import TextInput from './TextInput.vue'

    export default defineComponent({
        name: "EmailField",
        components:{
            TextInput
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
        <TextInput :model="val" field-type="email" :field-model="model" />
        <span v-if="model?.values?.$values?.length > 1" class="remove-field"  @click="formStore.removeMonolingualValue(model, val.id)"> x </span>
    </div>
</template>
