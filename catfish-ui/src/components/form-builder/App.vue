<script lang="ts">
    import { defineComponent, PropType, computed, watch} from "vue";
    import { Pinia } from 'pinia'
    import { Guid } from "guid-typescript";

    import { useFormEditorStore } from './store';
    import { default as Form } from './components/Form.vue';
    import { FieldType, FieldTypes } from '../shared/form-models';

    export default defineComponent({
        name: "FormEditor",
        components: {
            Form
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

            watch(() => store.transientMessage, async newMessage => {
                if (newMessage)
                    setTimeout(() => {
                        store.clearMessages();
                    }, 2000)
            })

            return {
                store,
                FieldTypes,
                newForm: () => store.newForm(),
                saveForm: () => store.saveForm(),
                hasForm: computed(() => store.form ? true : false),
            }
        }

    });
</script>

<style scoped src="./styles.css"></style>

<template>
    <transition name="fade">
        <p v-if="store.transientMessage" :class="'alert alert-' + store.transientMessageClass">{{store.transientMessage}}</p>
    </transition>
    <h2>Form Builder</h2>
    <div class="control">
        <button type="button" class="btn btn-primary" :disabled="hasForm" @click="newForm">New Form</button>
        <button type="button" class="btn btn-success" :disabled="!hasForm" @click="saveForm">Save</button>
    </div>
    <div class="toolbar">
        <button :disabled="!hasForm" @click="store.newField(FieldTypes.SingleLine)">+ TextField</button>
        <button :disabled="!hasForm" @click="store.newField(FieldTypes.Paragraph)">+ Paragraph</button>
        <button :disabled="!hasForm" @click="store.newField(FieldTypes.RichText)">+ Rich Text</button>
        <button :disabled="!hasForm" @click="store.newField(FieldTypes.Date)">+ Date</button>
        <button :disabled="!hasForm" @click="store.newField(FieldTypes.DateTime)">+ Date/Time</button>
        <button :disabled="!hasForm" @click="store.newField(FieldTypes.Decimal)">+ Decimal</button>
        <button :disabled="!hasForm" @click="store.newField(FieldTypes.Integer)">+ Integer</button>
        <button :disabled="!hasForm" @click="store.newField(FieldTypes.Email)">+ Email</button>
        <button :disabled="!hasForm" @click="store.newField(FieldTypes.CheckBoxes)">+ Checkboxes</button>
        <button :disabled="!hasForm" @click="store.newField(FieldTypes.DataList)">+ Data List</button>
        <button :disabled="!hasForm" @click="store.newField(FieldTypes.RadioButtons)">+ Radio Buttons</button>
        <button :disabled="!hasForm" @click="store.newField(FieldTypes.DropDown)">+ Drop Down</button>
        <button :disabled="!hasForm" @click="store.newField(FieldTypes.InfoSection)">+ Info Section</button>
    </div>
    <hr />
    {{store.form}}
    <Form v-if="store.form" :model="store.form" />
</template>

