
<script setup lang="ts">
    import { computed } from 'vue'

    import { Field, OptionFieldType, FieldTypes, FieldData } from '../../shared/form-models';

    import { useFormSubmissionStore } from '../store';
    import { default as TextCollection } from './TextCollection.vue'
    import * as formHelper from '../../shared/form-helpers'

    const props = defineProps<{ model: Field }>();
    const store = useFormSubmissionStore();

    const fieldData = computed(() => store.formData.fieldData?.find(fd => fd.fieldId == props.model.id) as FieldData)

</script>

<template>
    <select>
        <option v-for="opt in model.options" :key="opt.id">{{formHelper.getOptionText(opt, store.lang)}}</option>
    </select>
    {{fieldData}}
</template>

