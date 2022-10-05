
<script setup lang="ts">
    import { computed, ref } from 'vue'
    import { Guid } from 'guid-typescript'

    import * as formHelper from '../../shared/form-helpers'
    import { Field, OptionFieldType, FieldData } from '../../shared/form-models';
    import { useFormSubmissionStore } from '../store';
    import { default as TextCollection } from './TextCollection.vue'

    const props = defineProps<{ model: Field,
                                 modelData?: FieldData | null }>();
    const store = useFormSubmissionStore();

     const fieldData = computed(() => props.modelData? props.modelData :
                 store.formData.fieldData?.find(fd => fd.fieldId == props.model.id) as FieldData)

    const selectedOptionId = computed({
        get: () => fieldData?.value?.selectedOptionIds && fieldData.value.selectedOptionIds.length > 0 ? fieldData.value.selectedOptionIds[0] as unknown as string : Guid.EMPTY as unknown as string ,
        set: optId => fieldData.value.selectedOptionIds = [optId as unknown as Guid]
    })
</script>

<template>
    <div v-for="opt in model.options" :key="opt.id" class="option-field">
        <input type="radio" name="model.id" :value="(opt.id as unknown as string)" v-model="selectedOptionId" /> {{formHelper.getOptionText(opt, store.lang)}}
    </div>
</template>

