
<script setup lang="ts">
    import { Field, FieldType, FieldTypes } from '../../shared/form-models';
    import { useFormSubmissionStore } from '../store';
    import * as formHelper from '../../shared/form-helpers'
    import { default as CheckBoxes } from './CheckBoxes.vue'
    import { default as DataList } from './DataList.vue'
    import { default as DropDown } from './DropDown.vue'
    import { default as RadioButtons } from './RadioButtons.vue'

    const props = defineProps<{ model: Field }>();
    const store = useFormSubmissionStore();

    const title = formHelper.getFieldTitle(props.model, store.lang)
    const description = formHelper.getFieldDescription(props.model, store.lang)

</script>

<template>
    <div>
        <span class="fieldTitle">{{model.type}} - Title: {{title}}</span>
        <span class="fieldTitle">Description: {{description}}</span>

        <CheckBoxes :model="model" v-if="model.type === FieldTypes.CheckBoxes" />
        <DataList :model="model" v-if="model.type === FieldTypes.DataList" />
        <DropDown :model="model" v-if="model.type === FieldTypes.DropDown" />
        <RadioButtons :model="model" v-if="model.type === FieldTypes.RadioButtons" />
    </div>
</template>

