<script lang="ts">
    import { defineComponent, PropType } from 'vue'
    import { Field } from '../../models/fieldContainer'


    export default defineComponent({
        name: "Field",
        props: {
            model: null as PropType<Field> | null,
        },

    });
</script>

<template>
    <h3>Field</h3>
    <div>{{JSON.stringify(model)}}</div>
</template>