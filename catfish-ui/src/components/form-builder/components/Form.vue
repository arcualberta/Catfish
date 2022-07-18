<script lang="ts">
    import { defineComponent } from "vue";
    import { default as Field } from './Field.vue'
    import { default as TextCollection } from './TextCollection.vue'

    export default defineComponent({
        name: "Form",
        components: {
            Field
        }
    });
</script>

<script setup lang="ts">
    import { Form } from '../../shared/form-models';

    const props = defineProps<{ model: Form }>();

</script>

<template>
    <h3>Form properties</h3>
    <div>
        Name:
        <TextCollection v-if="model.name" :model="model.name" />
        <button v-else>Set name</button>
    </div>
    <div>
        Description:
        <TextCollection v-if="model.descrption" :model="model.descrption" />
        <button v-else>Set description</button>
    </div>

    <h3>Fields</h3>
    <Field v-for="field in model?.fields" :key="field.id" :model="field" />
</template>

