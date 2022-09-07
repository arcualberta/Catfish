
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
    import { default as InfoSection } from './InfoSection.vue'

    const props = defineProps<{ model: Field }>();
    const store = useFormSubmissionStore();

    const title = formHelper.getFieldTitle(props.model, store.lang)
    const description = formHelper.getFieldDescription(props.model, store.lang)
    const isMultilingualTextInputField = formHelper.isMultilingualTextInputField(props.model)
    const isMonolingualTextInputField = formHelper.isMonolingualTextInputField(props.model)
</script>

<template>
    <b-container>
        <!-- print field name and discription-->
        <b-row>

            <div v-if="model.type === FieldTypes.InfoSection" class="alert alert-info">
                <h3 class="text-field-lable">{{title}}</h3>
            </div>
            <b-col v-else class="col-sm-2">
                {{title}} <span class="hovertext" :data-hover="description" v-if="description"><font-awesome-icon icon="fas fa-question-circle" class="fas fa-question-circle" /></span> :
            </b-col>
            <b-col class="col-sm-10">
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
                <!-- InfoSection  field types -->
                <InfoSection :model="model" v-if="model.type === FieldTypes.InfoSection" />
            </b-col>
        </b-row>
        <br />
    </b-container>
</template>

