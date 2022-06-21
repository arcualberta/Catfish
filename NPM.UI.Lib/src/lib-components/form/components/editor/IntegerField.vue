<script lang="ts">
    import { defineComponent, PropType } from "vue";
    import * as models from '../../models'
    import TextInput from './TextInput.vue'

    export default defineComponent({
        name: "IntegerField",
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
    });
</script>


<template>
    <template>
        <div v-for="val in model?.values?.$values" :key="val.id">
            <TextInput :model="val" field-type="number" :field-model="model" />
            <span v-if="model?.values?.$values?.length > 1" class="fa remove-circle" style="margin-left: 30%; padding-right: 3px; padding-left: 2px; padding-bottom: 6px;" @click="formStore.removeMonolingualValue(model, val.id)"> x </span>
        </div>
    </template>
</template>
