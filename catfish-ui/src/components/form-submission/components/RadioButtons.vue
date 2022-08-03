
<script setup lang="ts">
    import { computed } from 'vue'

    import * as formHelper from '../../shared/form-helpers'
    import { Field, OptionFieldType, FieldTypes, FieldData } from '../../shared/form-models';
    import { useFormSubmissionStore } from '../store';
    import { default as TextCollection } from './TextCollection.vue'

    const props = defineProps<{ model: Field }>();
    const store = useFormSubmissionStore();

    const fieldData = computed(() => store.formData.fieldData?.find(fd => fd.fieldId == props.model.id) as FieldData)

</script>

<template>
    <div v-for="opt in model.options" :key="opt.id" class="option-field">
        <input type="radio" name="model.id" /> {{formHelper.getOptionText(opt, store.lang)}}
    </div>

    {{fieldData}}
</template>

