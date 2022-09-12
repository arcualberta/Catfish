<script setup lang="ts">
    import { computed, watch } from "vue";
    import { Pinia } from 'pinia'
    import { Guid } from "guid-typescript";

    import { useFormBuilderStore } from './store';
    import { Field } from '../shared/form-models'
    import { createTextCollection, isOptionField, createOption } from '../shared/form-helpers'
    import { FieldType } from '../shared/form-models';
    import { AppletAttribute } from '../shared/props'

    import { default as Form } from './components/Form.vue';

    const props = defineProps<{
        dataAttributes: AppletAttribute | null,
        queryParameters: AppletAttribute | null,
        piniaInstance: Pinia,
        repositoryRoot: string,
        formId?: Guid
    }>();

    const store = useFormBuilderStore(props.piniaInstance);

    if (props.formId)
        store.loadForm(props.formId)

    watch(() => store.transientMessage, async newMessage => {
        if (newMessage)
            setTimeout(() => {
                store.transientMessage = null;
            }, 2000)
    })

    const newForm = () => {
        store.form = {
            id: Guid.EMPTY as unknown as Guid,
            name: "",
            description: "",
            fields: [] as Field[]
        };
    }

    const saveForm = () => store.saveForm()

    const disabled = computed(() => store.form ? false : true)

    const newField = (fieldType: FieldType) => {

        const field = {
            id: Guid.create().toString() as unknown as Guid,
            title: createTextCollection(store.lang),
            description: createTextCollection(store.lang),
            type: fieldType,
        } as unknown as Field;

        if (isOptionField(field)) {
            field.options = [createOption(store.lang, null)]
        }
        store.form?.fields.push(field);
    }

</script>
<style scoped src="./styles.css"></style>

<template>
    <transition name="fade">
        <p v-if="store.transientMessage" :class="'alert alert-' + store.transientMessageClass">{{store.transientMessage}}</p>
    </transition>
    <h2>Form Builder</h2>
    <Form v-if="store.form" :model="store.form" />
    <div class="control">
        <button type="button" class="btn btn-primary" :disabled="!disabled" @click="newForm">New Form</button>
        <button type="button" class="btn btn-success" :disabled="disabled" @click="saveForm">Save</button>
    </div>
    <div class="toolbar">
        <button :disabled="disabled" @click="newField(FieldType.ShortAnswer)">+ Short Answer</button>
        <button :disabled="disabled" @click="newField(FieldType.Paragraph)">+ Paragraph</button>
        <button :disabled="disabled" @click="newField(FieldType.RichText)">+ Rich Text</button>
        <button :disabled="disabled" @click="newField(FieldType.Date)">+ Date</button>
        <button :disabled="disabled" @click="newField(FieldType.DateTime)">+ Date/Time</button>
        <button :disabled="disabled" @click="newField(FieldType.Decimal)">+ Decimal</button>
        <button :disabled="disabled" @click="newField(FieldType.Integer)">+ Integer</button>
        <button :disabled="disabled" @click="newField(FieldType.Email)">+ Email</button>
        <button :disabled="disabled" @click="newField(FieldType.Checkboxes)">+ Checkboxes</button>
        <button :disabled="disabled" @click="newField(FieldType.DataList)">+ Data List</button>
        <button :disabled="disabled" @click="newField(FieldType.RadioButtons)">+ Radio Buttons</button>
        <button :disabled="disabled" @click="newField(FieldType.DropDown)">+ Drop Down</button>
        <button :disabled="disabled" @click="newField(FieldType.InfoSection)">+ Info Section</button>
    </div>
    <hr />
    <!--{{store.form}}-->

</template>
