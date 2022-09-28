
<script setup lang="ts">
    import { computed } from 'vue'
    import { Guid } from 'guid-typescript'

    import { Field, OptionFieldType, FieldData } from '../../shared/form-models';
    import { useFormSubmissionStore } from '../store';
    import { default as CustomOptions } from './CustomOptions.vue'
    import { getTextValue } from '../../shared/form-helpers'
    import * as formHelper from '../../shared/form-helpers'
    const props = defineProps<{ model: Field,
                                modelData?: FieldData | null }>();
    const store = useFormSubmissionStore();

     const fieldData = computed(() => props.modelData? props.modelData :
                 store.formData.fieldData?.find(fd => fd.fieldId == props.model.id) as FieldData)


    const getOptionLabel = (optId: Guid | undefined): string => {
        const option = props.model?.options?.filter(opt => opt.id === optId).at(0);
        return option ? formHelper.getOptionText(option, store.lang) as string : "";
    }

    const getOptionId = (optLabel: string): Guid | undefined => {
        const option = props.model?.options?.filter(opt => formHelper.getOptionText(opt, store.lang) as string === optLabel).at(0);
        return option?.id;
    }

    const selectedOption = computed({
        get: () => getOptionLabel(fieldData?.value?.selectedOptionIds?.at(0)),
        set: optLabel => {
            const optId = getOptionId(optLabel);
            if (optId)
                fieldData.value.selectedOptionIds = [optId];
            else
                fieldData.value.selectedOptionIds = [];
        }

    })
</script>

<template>
    <div class="col-sm-8">
        <b-form-input list="dataOptions" id="model.id" name="model.id" v-model="selectedOption" />
        <datalist id="dataOptions">
            <option v-for="opt in model.options" :key="opt.id">{{formHelper.getOptionText(opt, store.lang)}}</option>
        </datalist>
    </div>
    <div class="col-sm-2">
        <CustomOptions :model="model" />
    </div>
    
</template>

