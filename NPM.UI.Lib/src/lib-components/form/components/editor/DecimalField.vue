<script lang="ts">
    import { defineComponent, PropType } from "vue";
    import * as models from '../../models'
    import TextInput from './TextInput.vue'


    export default defineComponent({
        name: "DecimalField",

        components: {
            TextInput
        },

        props: {
            model: {
                type: null as PropType<models.MonolingualTextField> | null,
                required: false
            },
            isMultivalued:
            {
                type: Boolean,
                required: false,
                default: false
            },
            numDecimalPlaces: {
                type: Number,
                required: false,
                default: 2
            }
        },
        setup(p) {

            return {
                numericStep: Math.pow(10, -p.numDecimalPlaces)
            }
        }
    });
</script>


<template>
    <div v-for="val in model?.values?.$values" :key="val.id">
        <TextInput :model="val" field-type="number" :field-model="model" numeric-step="numericStep" />
        <span v-if="model?.values?.$values?.length > 1" class="remove-field" @click="formStore.removeMonolingualValue(model, val.id)"> x </span>
    </div>
</template>
