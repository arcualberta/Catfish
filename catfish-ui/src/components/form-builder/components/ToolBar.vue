<script setup lang="ts">
import { Field, FieldType } from '@/components/shared/form-models';
import {storeToRefs} from 'pinia'
import { Guid } from 'guid-typescript';
import { useFormBuilderStore } from '../store';
import { createTextCollection, isOptionField, isTextInputField, createOption } from '../../shared/form-helpers'
  
const props = defineProps<{
        open: boolean,
        index: number
    }>();
const store = useFormBuilderStore();

const {form} = storeToRefs(store);


const newField = (fieldType: FieldType) => {

        const field = {
            id: Guid.create().toString() as unknown as Guid,
            title: createTextCollection(store.lang),
            description: createTextCollection(store.lang),
            type: fieldType,
        } as Field;

        //TODO: Restrict the following isMultiValued property only for monolingual and multilingual fields. We should leave
        //it undefined for other field types.
        if (isTextInputField(field)) {
            field.isMultiValued = false;
            field.isRequired = false;
            field.allowCustomOptionValues = false;
        }
        if (isOptionField(field)) {
            field.options = []
        }
       // store.form!.fields.push(field);
       store.form!.fields.splice(props.index, 0, field);
    }


</script>

<template>

    <div class="tool-modal" v-show="open">
    <div class="tool-modal-inner">
        
      <div class="tool-modal-content"> 
      <!--<div>
       <button type="button" @click="$emit('close')" class='closeModalBtn'>Close</button> 
        </div> -->
        
        <button  @click="newField(FieldType.ShortAnswer); $emit('close')">Short Answer</button>
        <button  @click="newField(FieldType.Paragraph);$emit('close')">Paragraph</button>
        <button  @click="newField(FieldType.RichText);$emit('close')">Rich Text</button>
        <button  @click="newField(FieldType.Date);$emit('close')">Date</button>
        <button  @click="newField(FieldType.DateTime);$emit('close')">Date/Time</button>
        <button  @click="newField(FieldType.Decimal);$emit('close')">Decimal</button>
        <button  @click="newField(FieldType.Integer);$emit('close')">Integer</button>
        <button  @click="newField(FieldType.Email);$emit('close')">Email</button>
        <button  @click="newField(FieldType.Checkboxes);$emit('close')">Checkboxes</button>
        <button  @click="newField(FieldType.DataList);$emit('close')">Data List</button>
        <button  @click="newField(FieldType.RadioButtons);$emit('close')">Radio Buttons</button>
        <button  @click="newField(FieldType.DropDown);$emit('close')">Drop Down</button>
        <button  @click="newField(FieldType.InfoSection);$emit('close')">Info Section</button>
        <button  @click="newField(FieldType.AttachmentField);$emit('close')">Attachment Field</button>
        
     </div>
     </div>
    </div>
</template>
<style scoped src="../styles.css"></style>
