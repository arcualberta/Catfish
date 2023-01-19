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

    }
    const resetFields = ()=>{
    }
</script>

<template>
    <div class="header-style">Email Templates <font-awesome-icon icon="fa-solid fa-circle-plus" style="color:#1ca5b8" @click="ToggleAddStates()"/></div>
     <ConfirmPopUp v-if="addTemplates" >
        <template v-slot:header>
            Add a State.
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
                    <QuillEditor v-model="templateBody" theme="snow"></QuillEditor>
                </b-input-group>
            </div>
        </template>
        <template v-slot:footer>
            <button type="button" class="modal-add-btn" aria-label="Close modal" :disabled="disabled" @click="addTemplate(templateId)">Add</button>
        </template>
    </ConfirmPopUp>
</template>

