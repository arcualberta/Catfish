<script setup lang="ts">
    import { Field, FieldType } from '../../shared/form-models';
    import { useFormSubmissionStore } from '../store';
    import * as formHelper from '../../shared/form-helpers'
    const props = defineProps<{ model: Field }>();
    const store = useFormSubmissionStore();
    const description = formHelper.getFieldDescription(props.model, store.lang)
</script>
<template>
    <div>
        <br />
        <div v-html="description" class="alert alert-info" />
    </div>
    

</template>