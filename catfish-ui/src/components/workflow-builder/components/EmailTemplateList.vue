<script setup lang="ts">
    import { EmailTemplate } from '../models'
    import { useWorkflowBuilderStore } from '../store';
    import {storeToRefs} from 'pinia';
    import {default as ConfirmPopUp} from "../../shared/components/pop-up/ConfirmPopUp.vue"
    import { watch, ref } from 'vue';
    import { Guid } from 'guid-typescript';
    import { QuillEditor } from '@vueup/vue-quill'
    import '@vueup/vue-quill/dist/vue-quill.snow.css'
    

    const store = useWorkflowBuilderStore();
    const emailTemplates = ref(store.workflow?.emailTemplates);
    const addTemplates = ref(false);
    const templateId = ref("");
    const templateName = ref("");
    const templateDescription = ref("");
    const templateSubject = ref("");
    const templateBody = ref("");
    let disabled = ref(true);
    const ToggleAddStates = () => (addTemplates.value = !addTemplates.value);

    watch(() => templateName.value, async newValue => {
        if (newValue.length>0)
            disabled.value = false; 
        else
            disabled.value = true; 
    })
    
    const addTemplate =(id:string)=>{
        if(id.length === 0){
            let newTemplate= {
                id:Guid.create(),
                name :templateName.value,
                description : templateDescription.value,
                emailSubject: templateSubject.value,
                emailBody: templateBody.value

            } as EmailTemplate;
        
            emailTemplates.value?.push(newTemplate)
        }else{
            const idx = emailTemplates.value?.findIndex(emt => emt.id.toString() == templateId.value)
            emailTemplates.value!.forEach((emt)=> {
                if(emt.id.toString() === templateId.value){
                    emt.name= templateName.value;
                    emt.description= templateDescription.value;
                    emt.emailSubject= templateSubject.value;
                    emt.emailBody= templateBody.value;
                }    
             })
        }
        resetFields();
        addTemplates.value = false;
    }
    const deleteTemplate = (templateId: Guid) => {
        const idx =emailTemplates.value?.findIndex(tmp => tmp.id == templateId)
        emailTemplates.value?.splice(idx as number, 1)
    }
    const editTemplate = (editTemplateId: Guid) => {
        const templateValues = emailTemplates.value?.filter(tmp => tmp.id == editTemplateId) as EmailTemplate[]
        templateName.value=templateValues[0].name 
        templateDescription.value = templateValues[0].description as string
        templateSubject.value = templateValues[0].emailSubject as string
        templateBody.value = templateValues[0].emailBody as string
        templateId.value = templateValues[0].id.toString()
        addTemplates.value = true
    }
    const resetFields = ()=>{
        templateId.value = "";
        templateName.value = "";
        templateDescription.value = "";
        templateSubject.value = "";
        templateBody.value = "";
    }
</script>

<template>
    <div class="list-item">
        <b-list-group>
            <b-list-group-item v-for="emailTemplate in emailTemplates" :key="emailTemplate.name">
                <span>{{emailTemplate.name}}</span>
                <span style="display:inline">
                    <font-awesome-icon icon="fa-solid fa-circle-xmark" style="color: red; float: right;" @click="deleteTemplate(emailTemplate.id as Guid)"/>
                    <font-awesome-icon icon="fa-solid fa-pen-to-square"  class="fa-icon" style="color: #007bff; float: right;" @click="editTemplate(emailTemplate.id as Guid)" />
                </span>
            </b-list-group-item>
        </b-list-group>
    </div>
    <div class="header-style">Email Templates <font-awesome-icon icon="fa-solid fa-circle-plus" style="color:#1ca5b8" @click="resetFields();ToggleAddStates()"/></div>
     <ConfirmPopUp v-if="addTemplates" >
        <template v-slot:header>
            Add an Email Template.
            <button type="button" class="btn-close" @click="addTemplates=false">x</button>
        </template>
        <template v-slot:body>
            <div >
                <b-input-group prepend="Name" class="mt-3">
                    <b-form-input v-model="templateName" ></b-form-input>
                </b-input-group>
                <b-input-group prepend="Description" class="mt-3">
                    <b-form-textarea v-model="templateDescription" rows="3" max-rows="6"></b-form-textarea>
                </b-input-group>
                <b-input-group prepend="Subject" class="mt-3">
                    <b-form-input v-model="templateSubject" ></b-form-input>
                </b-input-group>
                <b-input-group prepend="Email Body" class="mt-3">
                    <QuillEditor v-model="templateBody" theme="snow"  class="text-editor"></QuillEditor>
                </b-input-group>
            </div>
        </template>
        <template v-slot:footer>
            <button type="button" class="modal-add-btn" aria-label="Close modal" :disabled="disabled" @click="addTemplate(templateId)">Add</button>
        </template>
    </ConfirmPopUp>
</template>

