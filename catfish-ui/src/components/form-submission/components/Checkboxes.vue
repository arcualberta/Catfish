<script setup lang="ts">
    import { computed } from 'vue'
    import { Guid } from 'guid-typescript'
    import * as formHelper from '../../shared/form-helpers'
    import { Field, OptionFieldType, FieldData, Option } from '../../shared/form-models';
    import { useFormSubmissionStore } from '../store';
    import { default as CustomOptions } from './CustomOptions.vue'

    const props = defineProps<{ model: Field,
                                 modelData?: FieldData | null}>();
    const store = useFormSubmissionStore();

    const fieldData = computed(() => props.modelData? props.modelData :
                 store.formData.fieldData?.find(fd => fd.fieldId == props.model.id) as FieldData)

    const isSelected = (optId: Guid) => fieldData.value.selectedOptionIds?.includes(optId);
    const setSelection = (optId: Guid, checked: boolean) => checked
        ? fieldData.value.selectedOptionIds?.push(optId)
        : fieldData.value.selectedOptionIds?.splice(fieldData.value.selectedOptionIds?.indexOf(optId), 1);

</script>

<template>
    <div v-for="opt in model.options" :key="opt.id" class="option-field">
        <input type="checkbox" :checked="isSelected(opt.id)" @change="setSelection(opt.id, ($event.target as HTMLInputElement).checked)" /> {{formHelper.getOptionText(opt, store.lang)}}
        <span v-if="opt.isExtendedInput">
        </span>
    </div>
    <CustomOptions :model="model" />
</template>
