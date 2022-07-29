<script setup lang="ts">
    import { Field, FieldType, FieldTypes } from '../../shared/form-models';
    import { useFormSubmissionStore } from '../store';
    import * as formHelper from '../../shared/form-helpers'
    const props = defineProps<{ model: Field }>();
    const store = useFormSubmissionStore();
    const description = formHelper.getFieldDescription(props.model, store.lang)
</script>
<template>
    model:{{JSON.stringify(model)}}
    <div v-for="(txt, index) in description" :key="index" v-html="txt.value" />

</template>