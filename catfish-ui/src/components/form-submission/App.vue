<script setup lang="ts">
    import { computed, watch} from "vue";
    import { Pinia } from 'pinia'
    import { Guid } from "guid-typescript";
    import type { FormData } from '../shared/form-models'
    import { useFormSubmissionStore } from './store';
    import { default as Form } from './components/Form.vue';
    import { default as TransientMessage } from '../shared/components/transient-message/TransientMessage.vue'
    //import { FieldType } from '../shared/form-models';

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


    const saveForm = () => store.saveForm()
    const submitForm = () => {
        store.submitForm()
        .then(submissionStatus => {
            console.log("Response: ", submissionStatus)
            if(submissionStatus){
                //Remove the edit view of the form and display a message that the form submission is successful.
            }
            else{
                //Just stay here in the new-form  because the form submission failed
            }
        })
    }
    const hasForm = computed(() => store.form ? true : false)

    const formData = computed(() => store.formData as FormData | undefined)

</script>

<style scoped src="./styles.css"></style>

<template>
   <!-- <transition name="fade">
        <p v-if="store.transientMessage" :class="'alert alert-' + store.transientMessageClass">{{store.transientMessage}}</p>
    </transition>-->

    <h2>Form Submission</h2>
    <hr />
     <TransientMessage :model="store.transientMessageModel"></TransientMessage>
    <Form v-if="store.form" :model="store.form" :form-data="formData" />
    <div class="control">
        <!--<button type="button" class="btn btn-success" :disabled="!hasForm" @click="saveForm">Save</button>-->
        <button type="button" class="btn btn-primary" :disabled="!hasForm" @click="submitForm">Submit</button>
    </div>
<div>{{JSON.stringify(store.formData)}}</div>
</template>

