
<script setup lang="ts">
    import { Option, OptionFieldType, FieldTypes } from '../../shared/form-models';
    import { useFormEditorStore } from '../store';
    import { default as TextCollection } from './TextCollection.vue'

    const props = defineProps<{ model: Option, optionType: OptionFieldType }>();
    const store = useFormEditorStore();

</script>

<template>
    <span v-for="value in model.optionText.values">
        <span class="option-values"> {{value.lang}}: {{value.value}} </span>
    </span>
</template>

