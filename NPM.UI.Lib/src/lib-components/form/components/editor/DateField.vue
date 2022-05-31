<script lang="ts">
    import { defineComponent, PropType } from "vue";
    import * as models from '../../models'

    import SingleText from './SingleText.vue'

    export default defineComponent({
        name: "DateField",

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
            }
        },
    });
</script>


<template>
    <div v-for="val in model?.values?.$values" :key="val">
        <SingleText :model="val" :is-multiline="false"  field="date" />
    </div>


</template>
