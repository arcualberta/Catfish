<script lang="ts">
    import { defineComponent, PropType } from "vue";
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
                newForm: () => store.newForm(),
                newTextField: () => store.newField(FieldTypes.SingleLine as unknown as FieldType),
                newParagraph: () => store.newField(FieldTypes.Paragraph as unknown as FieldType),
                newRitchTextField: () => store.newField(FieldTypes.RichText as unknown as FieldType),
                newDateField: () => store.newField(FieldTypes.Date as unknown as FieldType),
                newDateTimeField: () => store.newField(FieldTypes.DateTime as unknown as FieldType),
            }
        }

    });
</script>


<template>
    <h2>Form Editor Component</h2>
    <button @click="newForm">New Form</button>
    <button @click="newTextField">+ TextField</button>
    <button @click="newParagraph">+ Paragraph</button>
    <button @click="newRitchTextField">+ Rich Text</button>
    <button @click="newDateField">+ Date</button>
    <button @click="newDateTimeField">+ Date/Time</button>

    <hr />
    {{store.form}}
</template>
