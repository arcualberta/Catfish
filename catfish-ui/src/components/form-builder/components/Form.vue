
<script setup lang="ts">
    import { Form, FieldTypes } from '../../shared/form-models';
    import { default as Field } from './Field.vue'
    import { default as TextCollection } from './TextCollection.vue'

    const props = defineProps<{ model: Form }>();
</script>

<template>
    <div>
        <h4>Form properties</h4>
    </div>
    <div>
        Name:
        <input type="text" v-model="model.name"  />
    </div>
    <div>
        Description:
        <textarea v-model="model.description"/>
    </div>

    <h3>Fields</h3>
    <Field v-for="field in model?.fields" :key="field.id" :model="field" />
</template>

