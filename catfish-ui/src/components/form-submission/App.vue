<script lang="ts">
    import { defineComponent, PropType, computed, watch} from "vue";
    import { Pinia } from 'pinia'
    import { Guid } from "guid-typescript";

    import { useFormSubmissionStore } from './store';
    import { default as Form } from './components/Form.vue';
    import { FieldType, FieldTypes } from '../shared/form-models';

    export default defineComponent({
        name: "FormSubmission",
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
            const store = useFormSubmissionStore(p.piniaInstance);

            if (p.formId)
                store.loadForm(p.formId);

            watch(() => store.transientMessage, async newMessage => {
                if (newMessage)
                    setTimeout(() => {
                        store.clearMessages();
                    }, 2000)
            })

            return {
                store,
                FieldTypes,
                saveForm: () => store.saveForm(),
                submitForm: () => store.submitForm(),
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
    <h2>Form Submission</h2>
    <div class="control">
        <button type="button" class="btn btn-success" :disabled="!hasForm" @click="saveForm">Save</button>
        <button type="button" class="btn btn-primary" :disabled="!hasForm" @click="submitForm">Submit</button>
    </div>
    <hr />
    {{store.form}}
    <Form v-if="store.form" :model="store.form" />
</template>

