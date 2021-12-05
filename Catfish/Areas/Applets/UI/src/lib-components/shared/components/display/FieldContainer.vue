<script lang="ts">
    import { defineComponent, PropType } from 'vue'

    import { FieldContainer } from '../../models/fieldContainer'
    import Field from './Field.vue'

    export default defineComponent({
        name: "FieldContainerView",
        props: {
            model: null as PropType<FieldContainer> | null,
        },
        components: {
            Field
        }
    });
</script>

<template>
    <h4>Field Container View</h4>
    <h4>{{model.name.concatenatedContent}}</h4>
    <div v-for="field in model.fields">
        <Field :model="field" />
    </div>
</template>