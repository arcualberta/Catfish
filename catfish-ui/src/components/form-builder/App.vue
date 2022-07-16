<script lang="ts">
    import { defineComponent, PropType, computed} from "vue";
    import { Pinia } from 'pinia'
    import { Guid } from "guid-typescript";

    import { useFormEditorStore } from './store';

    import { FieldType, FieldTypes } from '../shared/form-models';

    export default defineComponent({
        name: "FormEditor",
        components: {
        },
        props: {
            piniaInstance: {
                type: null as PropType<Pinia> | null,
                required: true
            },
            repositoryRoot: {
                type: null as PropType<string> | null,
                required: true
            },
            formId: {
                type: null as PropType<Guid> | null,
                required: false
            },
        },
        setup(p) {
            const store = useFormEditorStore(p.piniaInstance);

            if (p.formId) { }
            console.log(JSON.stringify(store))

            return {
                store,
                FieldTypes,
                newForm: () => store.newForm(),
                isDisabled: computed(() => !store.form),
            }
        }

    });
</script>


<template>
    <h2>Form Builder</h2>
    <button :disabled="!isDisabled" @click="newForm">New Form</button>
    <button :disabled="isDisabled" @click="store.newField(FieldTypes.SingleLine)">+ TextField</button>
    <button :disabled="isDisabled" @click="store.newField(FieldTypes.Paragraph)">+ Paragraph</button>
    <button :disabled="isDisabled" @click="store.newField(FieldTypes.RichText)">+ Rich Text</button>
    <button :disabled="isDisabled" @click="store.newField(FieldTypes.Date)">+ Date</button>
    <button :disabled="isDisabled" @click="store.newField(FieldTypes.DateTime)">+ Date/Time</button>
    <button :disabled="isDisabled" @click="store.newField(FieldTypes.Decimal)">+ Decimal</button>
    <button :disabled="isDisabled" @click="store.newField(FieldTypes.Integer)">+ Integer</button>
    <button :disabled="isDisabled" @click="store.newField(FieldTypes.Email)">+ Email</button>
    <button :disabled="isDisabled" @click="store.newField(FieldTypes.CheckBoxes)">+ Checkboxes</button>
    <button :disabled="isDisabled" @click="store.newField(FieldTypes.DataList)">+ Data List</button>
    <button :disabled="isDisabled" @click="store.newField(FieldTypes.RadioButtons)">+ Radio Buttons</button>
    <button :disabled="isDisabled" @click="store.newField(FieldTypes.DropDown)">+ Drop Down</button>
    <button :disabled="isDisabled" @click="store.newField(FieldTypes.InfoSection)">+ Info Section</button>
    <hr />
    {{store.form}}
</template>
