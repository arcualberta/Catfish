<script lang="ts">
    import { defineComponent, PropType, computed } from "vue";

    import * as models from '../../models'
   
    export default defineComponent({
        name: "InfoSection",
        props: {
            model: {
                type: null as PropType<models.InfoSection> | null,
                required: true
            },
         },
        setup(p) {

            const contents = computed((): models.Text[] => p.model.content?.values?.$values as models.Text[])

            return {
                contents
           }
        },
    });
</script>

<template>
    <div v-for="(txt, index) in contents" :key="index" v-html="txt.value" />
   <!-- {{JSON.stringify(model)}}-->
</template>
