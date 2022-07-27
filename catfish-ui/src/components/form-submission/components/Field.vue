
<script setup lang="ts">
    import { Field, FieldType, FieldTypes } from '../../shared/form-models';
    import { useFormSubmissionStore } from '../store';
    import * as formHelper from '../../shared/form-helpers'

    import { default as Checkboxes } from './Checkboxes.vue'
    import { default as DataList } from './DataList.vue'
    import { default as DropDown } from './DropDown.vue'
    import { default as RadioButtons } from './RadioButtons.vue'
    import { default as MultilingualTextInput } from './MultilingualTextInput.vue'
    import { default as MonolingualTextInput } from './MonolingualTextInput.vue'
    import { default as TextCollection } from './TextCollection.vue'

    const props = defineProps<{ model: Field }>();
    const store = useFormSubmissionStore();

    const title = formHelper.getFieldTitle(props.model, store.lang)
    const description = formHelper.getFieldDescription(props.model, store.lang)
    const isMultilingualTextInputField = formHelper.isMultilingualTextInputField(props.model)
    const isMonolingualTextInputField = formHelper.isMonolingualTextInputField(props.model)
</script>

<template>
    <div>
        <!-- print field name and discription-->
        <div>
            <span class="text-field-lable">{{title}}</span>:
            <span class="hovertext" :data-hover="description"><font-awesome-icon icon="fas fa-question-circle" class="fas fa-question-circle" /></span>
        </div>
        
        <!--<span class="fieldTitle">{{model.type}} - Title: {{title}}</span>
        <span class="fieldTitle">Description: {{description}}</span>-->

        <!-- Rendering appropriate user input field-->
        <!-- Option field types -->
        <Checkboxes :model="model" v-if="model.type === FieldTypes.Checkboxes" />
        <DataList :model="model" v-if="model.type === FieldTypes.DataList" />
        <DropDown :model="model" v-if="model.type === FieldTypes.DropDown" />
        <RadioButtons :model="model" v-if="model.type === FieldTypes.RadioButtons" />

        <!-- Multilingual Text Input field types -->
        <MultilingualTextInput :model="model" v-if="isMultilingualTextInputField" />

        <!-- Monolingual Text Input field types -->
        <MonolingualTextInput :model="model" v-if="isMonolingualTextInputField" />

        <br />
        <br />
    </div>
</template>

