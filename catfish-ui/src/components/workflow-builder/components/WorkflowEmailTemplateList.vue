<script setup lang="ts">
    import { WorkflowEmailTemplate } from '../models'
    import { useWorkflowBuilderStore } from '../store';
    import {default as ConfirmPopUp} from "../../shared/components/pop-up/ConfirmPopUp.vue"
    import { watch, ref } from 'vue';
    import { Guid } from 'guid-typescript';
    import { QuillEditor } from '@vueup/vue-quill'
    import '@vueup/vue-quill/dist/vue-quill.snow.css'
    

    const store = useWorkflowBuilderStore();
    const emailTemplates = ref(store.workflow?.emailTemplates);
    const addTemplates = ref(false);
    const template = ref({} as unknown as WorkflowEmailTemplate);
    let disabled = ref(true);
    const ToggleAddStates = () => (addTemplates.value = !addTemplates.value);

    watch(() => template.value.name, async newValue => {
        if (newValue.length>0)
            disabled.value = false; 
        else
            disabled.value = true; 
    })
    
    const addTemplate =(id:Guid)=>{
        if(id === Guid.EMPTY as unknown as Guid){
            let newTemplate= {
                id : Guid.create(),
                name : template.value.name,
                description : template.value.description,
                emailSubject : template.value.emailSubject,
                emailBody : template.value.emailBody

            } as WorkflowEmailTemplate;
            emailTemplates.value?.push(newTemplate)
        }else{
            emailTemplates.value!.forEach((emt)=> {
                if(emt.id === template.value.id){
                    emt.name = template.value.name;
                    emt.description = template.value.description;
                    emt.emailSubject = template.value.emailSubject;
                    emt.emailBody = template.value.emailBody;
                }    
             })
        }
        resetFields();
        addTemplates.value = false;
    }
    const deleteTemplate = (templateId: Guid) => {
        const idx = emailTemplates.value?.findIndex(tmp => tmp.id == templateId)
        emailTemplates.value?.splice(idx as number, 1)
    }
    const editTemplate = (editTemplateId: Guid) => {
        const templateValues = emailTemplates.value?.filter(tmp => tmp.id == editTemplateId) as WorkflowEmailTemplate[]
        template.value.name = templateValues[0].name 
        template.value.description = templateValues[0].description
        template.value.emailSubject = templateValues[0].emailSubject
        template.value.emailBody = templateValues[0].emailBody
        template.value.id = templateValues[0].id
        addTemplates.value = true
    }
    const resetFields = ()=>{
        template.value.id = Guid.EMPTY as unknown as Guid;
        template.value.name = "";
        template.value.description = "";
        template.value.emailSubject = "";
        template.value.emailBody = "";
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
                    <b-form-input v-model="template.name" ></b-form-input>
                </b-input-group>
                <b-input-group prepend="Description" class="mt-3">
                    <b-form-textarea v-model="(template.description as string)" rows="3" max-rows="6"></b-form-textarea>
                </b-input-group>
                <b-input-group prepend="Subject" class="mt-3">
                    <b-form-input v-model="template.emailSubject" ></b-form-input>
                </b-input-group>
                <b-input-group prepend="Email Body" class="mt-3">
                    <QuillEditor v-model:content="template.emailBody" contentType="html" theme="snow"  class="text-editor"></QuillEditor>
                </b-input-group>
            </div>
        </template>
        <template v-slot:footer>
            <button type="button" class="modal-add-btn" aria-label="Close modal" :disabled="disabled" @click="addTemplate(template.id as unknown as Guid)">Add</button>
        </template>
    </ConfirmPopUp>
</template>

