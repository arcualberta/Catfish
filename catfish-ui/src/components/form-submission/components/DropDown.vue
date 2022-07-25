
<script setup lang="ts">
    import { Field, OptionFieldType, FieldTypes } from '../../shared/form-models';
    import { useFormSubmissionStore } from '../store';
    import { default as TextCollection } from './TextCollection.vue'

    const props = defineProps<{ model: Field }>();
    const store = useFormSubmissionStore();

</script>

<template>
    {{model.options}}
    <select>
        <option v-for="opt in model.options" :key="opt.id" /> {{model.type}}
    </select>
</template>

