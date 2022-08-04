<script setup lang="ts">
    import { computed } from 'vue'
    import * as formHelper from '../../shared/form-helpers'
    import { Field, OptionFieldType, FieldTypes, FieldData, ExtensionType, Option } from '../../shared/form-models';
    import { useFormSubmissionStore } from '../store';
    import { default as CustomOptions } from './CustomOptions.vue'

    const props = defineProps<{ model: Field }>();
    const store = useFormSubmissionStore();
    const fieldData = computed(() => store.formData.fieldData?.find(fd => fd.fieldId == props.model.id) as FieldData)
</script>

<template>
    <div v-for="opt in model.options" :key="opt.id" class="option-field">
        <input type="checkbox" /> {{formHelper.getOptionText(opt, store.lang)}}
        <span v-if="opt.isExtendedInput != ExtensionType.None">
            TODO:
            <!--<input v-if="opt.isExtendedInput === ExtensionType.Required" type="text" required />
        <input v-else type="text" />-->
        </span>
    </div>
    {{fieldData}}
    <CustomOptions :model="model" />
</template>

