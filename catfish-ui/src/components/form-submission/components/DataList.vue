
<script setup lang="ts">
    import { computed } from 'vue'

    import { Field, OptionFieldType, FieldTypes, FieldData } from '../../shared/form-models';
    import { useFormSubmissionStore } from '../store';
    import { default as CustomOptions } from './CustomOptions.vue'
    import { getTextValue } from '../../shared/form-helpers'

    const props = defineProps<{ model: Field }>();
    const store = useFormSubmissionStore();

    const fieldData = computed(() => store.formData.fieldData?.find(fd => fd.fieldId == props.model.id) as FieldData)

</script>

<template>
    <input list="dataOptions" id="model.id" name="model.id" />
    <datalist id="dataOptions">
        <option v-for="opt in model.options" :key="opt.id" :value="getTextValue(opt.optionText, store.lang)" />
    </datalist>
    {{fieldData}}
    <CustomOptions :model="model" />
</template>

