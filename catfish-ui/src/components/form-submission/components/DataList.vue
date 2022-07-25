
<script setup lang="ts">
    import { Field, OptionFieldType, FieldTypes } from '../../shared/form-models';
    import { useFormSubmissionStore } from '../store';
    import { default as TextCollection } from './TextCollection.vue'
    import { getTextValue } from '../../shared/form-helpers'

    const props = defineProps<{ model: Field }>();
    const store = useFormSubmissionStore();

</script>

<template>
    <input list="dataOptions" id="model.id" name="model.id" />
    <datalist id="dataOptions">
        <option v-for="opt in model.options" :key="opt.id" :value="getTextValue(opt.optionText, store.lang)" />
    </datalist>
</template>

