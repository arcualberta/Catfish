<script lang="ts">
    import { defineComponent, PropType } from 'vue'
    import { Text } from '../../../models/textModels'

    export default defineComponent({
        name: "Text",
        props: {
            model: {
                type: null as PropType<Text> | null,
                required: true
           }
        }
    });
</script>

<template>
    <div>{{model.value}}</div>
</template>

