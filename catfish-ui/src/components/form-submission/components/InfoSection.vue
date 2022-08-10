<script setup lang="ts">
    import { Field, FieldType, FieldTypes } from '../../shared/form-models';
    import { useFormSubmissionStore } from '../store';
    import * as formHelper from '../../shared/form-helpers'
    const props = defineProps<{ model: Field }>();
    const store = useFormSubmissionStore();
    const description = formHelper.getFieldDescription(props.model, store.lang)
</script>
<template>
    <div>
        <br />
        <b-alert show>Default Alert</b-alert>
        <div v-html="description" class="alert alert-info" />
    </div>
    

</template>