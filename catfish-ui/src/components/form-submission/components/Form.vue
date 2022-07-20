
<script setup lang="ts">
    import { Guid } from "guid-typescript";

    import { Form, FieldTypes } from '../../shared/form-models';
    import { default as Field } from './Field.vue'
    import { default as TextCollection } from './TextCollection.vue'

    const props = defineProps<{ model: Form }>();
</script>

<template>
    <h3>Form properties</h3>
    <div>
        Name:
        <TextCollection v-if="model.name" :model="model.name" :text-type="FieldTypes.SingleLine" />
        <button v-else>Set name</button>
    </div>
    <div>
        Description:
        <TextCollection v-if="model.description" :model="model.description" :text-type="FieldTypes.Paragraph" />
        <button v-else>Set description</button>
    </div>

    <h3>Fields</h3>
    <Field v-for="field in model?.fields" :key="field.id" :model="field" />
</template>

