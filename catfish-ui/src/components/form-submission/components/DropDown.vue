
<script setup lang="ts">
    import { Field, OptionFieldType, FieldTypes } from '../../shared/form-models';

    import { useFormSubmissionStore } from '../store';
    import { default as TextCollection } from './TextCollection.vue'
    import { getTextValue } from '../../shared/form-helpers'

    const props = defineProps<{ model: Field }>();
    const store = useFormSubmissionStore();

</script>

<template>
    <select>
        <option v-for="opt in model.options" :key="opt.id">{{getTextValue(opt.optionText, store.lang)}}</option>
    </select>
</template>

