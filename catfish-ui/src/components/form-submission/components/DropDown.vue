
<script setup lang="ts">
    import { computed } from 'vue'
    import { Guid } from 'guid-typescript'
    import { Field, OptionFieldType, FieldTypes, FieldData } from '../../shared/form-models';

    import { useFormSubmissionStore } from '../store';
    import { default as TextCollection } from './TextCollection.vue'
    import * as formHelper from '../../shared/form-helpers'

    const props = defineProps<{ model: Field }>();
    const store = useFormSubmissionStore();

    const fieldData = computed(() => store.formData.fieldData?.find(fd => fd.fieldId == props.model.id) as FieldData)
    fieldData.value.selectedOptionIds = ["b0da267c-de92-41ef-02f3-67065eac7e18" as unknown as Guid]
    const selectedOptionId = computed({
        get: () => fieldData?.value?.selectedOptionIds && fieldData.value.selectedOptionIds.length > 0 ? fieldData.value.selectedOptionIds[0] : Guid.EMPTY,
        set: optId => fieldData.value.selectedOptionIds = [optId as unknown as Guid]
    })
</script>

<template>
    <select v-model="selectedOptionId">
        <option v-for="opt in model.options" :key="opt.id" :value="opt.id">{{formHelper.getOptionText(opt, store.lang)}}</option>
    </select>
    {{fieldData}}
</template>

