
<script setup lang="ts">
    import { Form, FieldTypes } from '../../shared/form-models';
    import { default as Field } from './Field.vue'
    import { default as TextCollection } from './TextCollection.vue'

    const props = defineProps<{ model: Form }>();
</script>

<template>
    <h3>Form properties</h3>
    <div>
        Name:
        <input type="text" />
    </div>
    <div>
        Description:
        <textarea />
    </div>

    <h3>Fields</h3>
    <Field v-for="field in model?.fields" :key="field.id" :model="field" />
</template>

