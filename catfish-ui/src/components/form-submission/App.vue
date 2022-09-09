<script setup lang="ts">
    import { defineComponent, PropType, computed, watch} from "vue";
    import { Pinia } from 'pinia'
    import { Guid } from "guid-typescript";

    import { useFormSubmissionStore } from './store';
    import { default as Form } from './components/Form.vue';
    import { FieldType } from '../shared/form-models';

    const props = defineProps<{
        piniaInstance: Pinia,
        repositoryRoot: string,
        formId?: Guid,
        submissionId?: Guid
    }>();
    const store = useFormSubmissionStore(props.piniaInstance);


    if (props.formId)
        store.loadForm(props.formId)
    else if (props.submissionId)
        store.loadSubmission(props.submissionId)

    watch(() => store.transientMessage, async newMessage => {
        if (newMessage)
            setTimeout(() => {
                store.transientMessage = null;
            }, 2000)
    })

    const saveForm = () => store.saveForm()
    const submitForm = () => store.submitForm()
    const hasForm = computed(() => store.form ? true : false)

</script>

<style scoped src="./styles.css"></style>

<template>
    <transition name="fade">
        <p v-if="store.transientMessage" :class="'alert alert-' + store.transientMessageClass">{{store.transientMessage}}</p>
    </transition>
    <h2>Form Submission</h2>
    <hr />
    <!--{{store.form}}-->
    <Form v-if="store.form" :model="store.form" />
    <div class="control">
        <!--<button type="button" class="btn btn-success" :disabled="!hasForm" @click="saveForm">Save</button>-->
        <button type="button" class="btn btn-primary" :disabled="!hasForm" @click="submitForm">Submit</button>
    </div>

</template>

