
<script setup lang="ts">
    import { Field, OptionFieldType, FieldTypes } from '../../shared/form-models';
    import { useFormSubmissionStore } from '../store';
    import { default as TextCollection } from './TextCollection.vue'

    const props = defineProps<{ model: Field }>();
    const store = useFormSubmissionStore();
</script>

<template>
    TODO: Multilingual Text Input
</template>

