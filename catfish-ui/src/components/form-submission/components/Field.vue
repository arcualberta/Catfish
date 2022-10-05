
<script setup lang="ts">
    import { ref } from 'vue'
    import { Field, FieldData, FieldType } from '../../shared/form-models';
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
    import {default as AttachmentField} from './AttachmentField.vue'
    const props = defineProps<{ model: Field,
                                modelData?: FieldData | null}>();
    const store = useFormSubmissionStore();

    const title = formHelper.getFieldTitle(props.model, store.lang)
    const description = formHelper.getFieldDescription(props.model, store.lang)
    const isMultilingualTextInputField = formHelper.isMultilingualTextInputField(props.model)
    const isMonolingualTextInputField = formHelper.isMonolingualTextInputField(props.model)
//
    const isAttachmentField = props.model.type === FieldType.AttachmentField ? true: false;
    const dropzoneFile=ref("");
    const fieldElementId=props.model.id.toString();
    const drop=(e)=>{
            dropzoneFile.value= e.dataTransfer.files[0];
    };

    const selectedFile=(fieldId)=>{
        dropzoneFile.value=document.getElementById(fieldId).files[0];
    }
</script>

<template>
    <b-container>
        <!-- print field name and discription-->
        <b-row>

            <div v-if="model.type === FieldType.InfoSection" class="alert alert-info">
                <h3 class="text-field-label">{{title}}</h3>
            </div>
            <b-col v-else class="col-sm-2">
                {{title}} <span class="hovertext" :data-hover="description" v-if="description"><font-awesome-icon icon="fas fa-question-circle" class="fas fa-question-circle" /></span> :
            </b-col>
            <b-col class="col-sm-10">
                <!-- Rendering appropriate user input field-->
                <!-- Option field types -->
                <Checkboxes :model="model" :modelData="modelData" v-if="model.type === FieldType.Checkboxes" />
                <DataList :model="model" :modelData="modelData" v-if="model.type === FieldType.DataList" />
                <DropDown :model="model" :modelData="modelData" v-if="model.type === FieldType.DropDown" />
                <RadioButtons :model="model" :modelData="modelData" v-if="model.type === FieldType.RadioButtons" />
                <!-- Multilingual Text Input field types -->
                <MultilingualTextInput :model="model" :modelData="modelData" v-if="isMultilingualTextInputField" />
                <!-- Monolingual Text Input field types -->
                <MonolingualTextInput :model="model" :modelData="modelData" v-if="isMonolingualTextInputField" />
                <!-- InfoSection  field types -->
                <InfoSection :model="model" v-if="model.type === FieldType.InfoSection" />
               <div v-if="isAttachmentField">
                  <AttachmentField :model="model" :elementId="fieldElementId" @drop="drop" @change="selectedFile(fieldElementId)" />
                  <span class="dropzoneFiles">Selected File: {{dropzoneFile.name}}</span>
                </div>
            </b-col>
        </b-row>
        <br />
    </b-container>
</template>

