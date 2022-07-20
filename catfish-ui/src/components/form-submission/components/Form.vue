
<script setup lang="ts">
    import { Guid } from "guid-typescript";

    import { Form } from '../../shared/form-models';
    import { default as Field } from './Field.vue'

    const props = defineProps<{ model: Form }>();
</script>

<template>
    <Field v-for="field in model?.fields" :key="field.id" :model="field" />
</template>

