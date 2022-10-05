
<script setup lang="ts">
    import { computed } from 'vue'
    import { Guid } from 'guid-typescript'
    import { Field, OptionFieldType, FieldData } from '../../shared/form-models';

    import { useFormSubmissionStore } from '../store';
    import { default as TextCollection } from './TextCollection.vue'
    import * as formHelper from '../../shared/form-helpers'

    const props = defineProps<{ model: Field,
                                modelData?: FieldData | null }>();
    const store = useFormSubmissionStore();

    //const fieldData = computed(() => store.formData.fieldData?.find(fd => fd.fieldId == props.model.id) as FieldData)
     const fieldData = computed(() => props.modelData? props.modelData :
                 store.formData.fieldData?.find(fd => fd.fieldId == props.model.id) as FieldData)

    const selectedOptionId = computed({
        get: () => fieldData?.value?.selectedOptionIds && fieldData.value.selectedOptionIds.length > 0 ? fieldData.value.selectedOptionIds[0] as unknown as string : Guid.EMPTY as unknown as string,
        set: optId => fieldData.value.selectedOptionIds = [optId as unknown as Guid]
    })
</script>

<template>
    <div class="col-sm-3">
        <select v-model="selectedOptionId" class="form-select">
            <option v-for="opt in model.options" :key="opt.id" :value="(opt.id as unknown as string)">{{formHelper.getOptionText(opt, store.lang)}}</option>
        </select>
    </div>
    
</template>

