<script lang="ts">
    import { defineComponent, PropType } from 'vue'
    import { FieldContainer } from '../../models/fieldContainer'
    import Field from './Field.vue'

    export default defineComponent({
		name: "FieldContainer",
        props: {
            model: null as PropType<FieldContainer> | null,
        },
        components: {
            Field
        },
    });
</script>

<template>
    <Field v-for="field in model.fields?.$values" :model="field" />
</template>