<script lang="ts">
    import { defineComponent, PropType } from "vue";
    import * as models from '../../models'
    import SingleText from './SingleText.vue'

    export default defineComponent({
        name: "IntegerField",
        components: {
            SingleText
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
            <SingleText :model="val" :is-multiline="false" :allow-delete="model?.values?.$values?.length > 1"  field="number" :fieldModel="model" />
        </div>
    </template>
</template>
