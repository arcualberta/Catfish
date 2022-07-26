
<script setup lang="ts">
    import { computed } from 'vue'

    import { Field, OptionFieldType, FieldTypes } from '../../shared/form-models';
    import { useFormSubmissionStore } from '../store';
    import { default as TextCollection } from './TextCollection.vue'

    const props = defineProps<{ model: Field }>();
    const store = useFormSubmissionStore();

    const fieldData = computed(() => store.formData.fieldData?.find(fd => fd.fieldId == props.model.id) as FieldData)

</script>

<template>
    {{fieldData}}
</template>

