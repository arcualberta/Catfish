<script lang="ts">
	

    import { Guid } from 'guid-typescript'
    import {  defineComponent } from 'vue';
    import { /*getActivePinia,*/ createPinia } from 'pinia'
  

    import { form, FormSubmission as SubmissionForm } from '@arcualberta/catfish-ui';

    import { useFormStore } from './store/FormStore';
    import props,{ DataAttribute } from '../shared/props'

    export default defineComponent({
        name: "FormSubmission",
       props,
		modules: {
			form
        },
		components: {
            SubmissionForm
        },
     
        setup(p) {

            
         
          
            console.log("setup")
            const pinia = createPinia();// getActivePinia();
            console.log(JSON.stringify(pinia))
            const dataAttributes = p.dataAttributes as DataAttribute;

            const itemTemplateId = Guid.parse(dataAttributes["template-id"] as string);
            const formId = Guid.parse(dataAttributes["form-id"] as string);
            const collectionId = Guid.parse(dataAttributes["collection-id"] as string);
            const groupId = dataAttributes["group-id"] ? Guid.parse(dataAttributes["group-id"] as string) : null;
            const apiServiceRoot = dataAttributes["site-url"] as string;

              const formStore =useFormStore(pinia)

           formStore.setFormId(formId);
            formStore.setTemplateId(itemTemplateId);
            formStore.setCollectionId(collectionId);
            if (groupId)
                formStore.setGroupId(groupId);
            formStore.setApiRoot(apiServiceRoot);


           // console.log("settings: api: " + formStore.apiRoot + " - frmId: " + formStore.formId + " -tmpId:  " + formStore.itemTemplateId + " - collId" + formStore.collectionId)
            formStore.fetchData();
            const submitForm = () => {
                
                if (form.helpers.validateForm(formStore.form as form.models.FieldContainer)) {
                   // console.log('Submitting form ...')
                    formStore.submitForm();
                }
                else
                    console.log('Form validation failed ...')
            }
            return {
               formStore,
               submitForm,
               pinia,
            }
        },
       
		
    });
</script>

<template>
    <div class="page-body">
         <SubmissionForm :model="formStore.form" :pinia-instance="pinia" />
         <button class="submit-button" @click="submitForm()">Submit</button>
    <div v-if="formStore.submissionFailed" class="alert alert-danger">Sorry, the form submission failed.</div>
       

    </div>
</template>
