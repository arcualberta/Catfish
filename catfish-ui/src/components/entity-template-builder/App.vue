<script setup lang="ts">
    import { Pinia } from 'pinia'
    import { computed, onActivated, onMounted, onUpdated, ref, toRef, watch } from 'vue'
    import { useEntityTemplateBuilderStore } from './store';
    import { AppletAttribute } from '@/components/shared/props'
    import { default as FormEntryTemplate } from './components/FormEntry.vue';
    import { Guid } from 'guid-typescript';
    import { useRoute ,useRouter } from 'vue-router';
   
    import { VueDraggableNext as draggable } from 'vue-draggable-next'
    import { FormEntry } from '../shared';
    import { default as TransientMessage } from '../shared/components/transient-message/TransientMessage.vue'
    import { FormFieldSelectionDropdown } from '@/components/shared/components'
    import { FieldEntry, FormTemplate } from '../shared/form-models';
    
    //import {useLoginStore} from '../login/store'

    const props = defineProps<{
        dataAttributes?: AppletAttribute | null,
        queryParameters?: AppletAttribute | null,
        apiRoot?: string |null,
        jwtToken?: string | null
     
    }>();

    const store = useEntityTemplateBuilderStore();
   // const authorizeStore = useLoginStore();

   // const _dataAttributes = toRef(props, 'dataAttributes')
   // const userJwtToken = _dataAttributes && _dataAttributes?.value? (_dataAttributes.value["UserJwtToken"] as string) : null;
    
    //DEBUG
   // console.log("token if sigin from piranha site:")
    //console.log(userJwtToken);

    //const jwtToken = authorizeStore.jwtToken;

    //DEBUG
    //console.log("token if sigin from vue login:")
    //console.log(jwtToken);

    if(props.apiRoot){
        store.setApiRoot(props.apiRoot);
    }

    if(props.jwtToken)
    {
        //localStorage.setItem("catfishJwtToken", props.jwtToken);
        store.jwtToken = props.jwtToken as string;
    }
    const createTemplate = () => store.newTemplate();

    const template = computed(() => store.template);
    const titleField = computed(() => template.value?.entityTemplateSettings.titleField);
    const descriptionField = computed(() => template.value?.entityTemplateSettings.descriptionField);
    const mediaField = computed(() => template.value?.entityTemplateSettings.mediaField);

    const formFieldSelectorSource = computed(() => [{ formGroupName: 'Matadata Form', formGroup: template.value?.entityTemplateSettings.metadataForms?.filter(form => form.isRequired) },
        { formGroupName: 'Data Form', formGroup: template.value?.entityTemplateSettings.dataForms?.filter(form => form.isRequired) }])
    const router = useRouter();

    const addMetadataForm = () => {
        store.template?.entityTemplateSettings.metadataForms?.push({ id: Guid.create().toString() as unknown as Guid, formId: Guid.createEmpty(), name: "" } as FormEntry);
    }

    const addDataForm = () => {
        store.template?.entityTemplateSettings.dataForms?.push({ id: Guid.create().toString() as unknown as Guid, formId: Guid.createEmpty(), name: "" } as FormEntry);
    }

    const saveTemplate = () => store.saveTemplate();
    let btnClasses="btn btn-primary";
    const route = useRoute()
    const templateId = route.params.id as unknown as Guid
  
    if(templateId){
      
       store.loadTemplate(templateId);
    }
    watch(() => titleField?.value?.formId, newVal => {
        store.associateForm(newVal as unknown as Guid)
    })

     watch(() => descriptionField?.value?.formId, newVal => {
        store.associateForm(newVal as unknown as Guid)
    })
     watch(() => mediaField?.value?.formId, newVal => {
        store.associateForm(newVal as unknown as Guid)
    })


    onMounted(() => {
         store.newTemplate();
        store.loadFormEntries();
        if (template.value) {
            if (template.value.id?.toString() !== Guid.EMPTY){
                store.loadTemplate(template.value.id as Guid)
               
            }
        }
    });
    
</script>

<template>
    <TransientMessage :model="store.transientMessageModel"></TransientMessage>
   
    <div class="control">
     <!--    <button class="btn btn-primary" @click="createTemplate" v-if="isNewTemplate">New Template</button> -->
        <button class="btn btn-success" @click="saveTemplate">Save</button>
    </div>
    <br />
    <div v-if="template">
        <div class="form-field-border">
            <b-row>
                <b-col class="col-sm-2">
                    <h6>Name :</h6>
                </b-col>
                <b-col class="col-sm-10">
                    <b-form-input v-model="template.name"></b-form-input>
                </b-col>
            </b-row>
            <br />
            <b-row>
                <b-col class="col-sm-2">
                    <h6>Description :</h6>
                </b-col>
                <b-col class="col-sm-10">
                    <b-form-textarea v-model="template.description"></b-form-textarea>
                </b-col>
            </b-row>
            <br />
            <b-row>
                <b-col class="col-sm-2">
                    <h6>State:</h6>
                </b-col>
                <b-col class="col-sm-10">
                    <h6>{{template.state}}</h6>
                </b-col>
            </b-row>
        </div>

        <div>
            <div class="form-field-border blue">
                <h5>Metadata Forms</h5>
                <draggable class="dragArea list-group w-full" :list="template.entityTemplateSettings.metadataForms">
                    <div v-for="frm in template.entityTemplateSettings.metadataForms" :key="frm.id.toString()">
                        <FormEntryTemplate :model="(frm as FormEntry)" />
                       
                    </div>
                </draggable>
                <button class="btn btn-primary btn-blue" @click="addMetadataForm">+ Add</button>
            </div>
        </div>
        <div class="form-field-border red">
            <h5>Data Forms</h5>
            <draggable class="dragArea list-group w-full" :list="template.entityTemplateSettings.dataForms">
                <div v-for="frm in template.entityTemplateSettings.dataForms" :key="frm.id.toString()">
                    <FormEntryTemplate :model="(frm as FormEntry)" class="form-field-border form-field red" />
                </div>
            </draggable>
            <button class="btn btn-warning btn-red" @click="addDataForm">+ Add</button>
        </div>
        <div v-if="template.forms">
            <h5>Field Mappings</h5>
            <div class="row">
                <div class="col-2">
                    Title
                </div>
                
                <div class="col-10">
                    <FormFieldSelectionDropdown :model="titleField" :option-source="formFieldSelectorSource" :forms="store.forms" />
                
                </div>
            </div>
            <div class="row">
                <div class="col-2">
                    Description
                </div>
                <div class="col-10">
                    <FormFieldSelectionDropdown :model="(descriptionField as FieldEntry)" :option-source="formFieldSelectorSource" :forms="store.forms" />
                </div>
            </div>
             <div class="row">
                <div class="col-2">
                    Media
                </div>
                <div class="col-10">
                    <FormFieldSelectionDropdown :model="mediaField" :option-source="formFieldSelectorSource" :forms="store.forms" />
                </div>
            </div>
        </div>
    </div>
  <!--  <div class="alert alert-info" style="margin-top:2em;">{{template}}</div> -->

   

</template>


<style scoped src="./style.css"></style>
