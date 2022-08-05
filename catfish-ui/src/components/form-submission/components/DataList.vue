
<script setup lang="ts">
    import { computed } from 'vue'
    import { Guid } from 'guid-typescript'

    import { Field, OptionFieldType, FieldTypes, FieldData } from '../../shared/form-models';
    import { useFormSubmissionStore } from '../store';
    import { default as CustomOptions } from './CustomOptions.vue'
    import { getTextValue } from '../../shared/form-helpers'
    import * as formHelper from '../../shared/form-helpers'
    const props = defineProps<{ model: Field }>();
    const store = useFormSubmissionStore();

    const fieldData = computed(() => store.formData.fieldData?.find(fd => fd.fieldId == props.model.id) as FieldData)
    //fieldData.value.selectedOptionIds = ["12f22208-da0b-f08f-75b7-39966420cb46" as unknown as Guid]
    const selectedOptionId = computed({
        get: () => fieldData?.value?.selectedOptionIds && fieldData.value.selectedOptionIds.length > 0 ? fieldData.value.selectedOptionIds[0] : Guid.EMPTY,
        set: optId => fieldData.value.selectedOptionIds = [optId as unknown as Guid]
    })
</script>

<template>
    <input list="dataOptions" id="model.id" name="model.id" @change="alert('Test')" />
    <datalist id="dataOptions" >
        <option v-for="opt in model.options" :key="opt.id" >{{formHelper.getOptionText(opt, store.lang)}}</option>
    </datalist>
    {{fieldData}}
    <CustomOptions :model="model" />
</template>

