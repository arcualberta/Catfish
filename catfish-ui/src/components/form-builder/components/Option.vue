
<script setup lang="ts">
    import { Option, OptionFieldType, FieldTypes } from '../../shared/form-models';
    import { useFormEditorStore } from '../store';
    import { default as TextCollection } from './TextCollection.vue'

    const props = defineProps<{ model: Option, optionType: OptionFieldType }>();
    const store = useFormEditorStore();</script>

<template>
    <h6>Option</h6>
    <!--{{model}}-->
    {{optionType}}
    <TextCollection v-if="model.optionText" :model="model.optionText" :text-type="FieldTypes.SingleLine" />
</template>

