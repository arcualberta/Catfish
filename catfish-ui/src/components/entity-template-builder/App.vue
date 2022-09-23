<script setup lang="ts">
    import { Pinia } from 'pinia'
    import { computed, onMounted } from 'vue'
    import { useEntityTemplateBuilderStore } from './store';
    import { AppletAttribute } from '@/components/shared/props'
    import { default as FormEntryTemplate } from './components/FormEntry.vue';
    import { Guid } from 'guid-typescript';
    import { useRoute ,useRouter } from 'vue-router';
   
    import { VueDraggableNext as draggable } from 'vue-draggable-next'
    import { FieldEntry, FormEntry } from '../shared/form-models';

    import { FormFieldSelectionDropdown } from '@/components/shared/components'

    const props = defineProps<{
        dataAttributes?: AppletAttribute | null,
        queryParameters?: AppletAttribute | null,
        piniaInstance: Pinia
    }>();

    const store = useEntityTemplateBuilderStore(props.piniaInstance);
    const createTemplate = () => store.newTemplate();

    const template = computed(() => store.template);
    const titleField = computed(() => template.value?.entityTemplateSettings.titleField);
    const descriptionField = computed(() => template.value?.entityTemplateSettings.descriptionField);

    const formFieldSelectorSource = computed(() => [{ formGroupName: 'Matadata Form', formGroup: template.value?.entityTemplateSettings.metadataForms }, { formGroupName: 'Data Form', formGroup: template.value?.entityTemplateSettings.dataForms }])
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
    const templateId = route.params.templateId as unknown as Guid
    if(templateId)
       btnClasses="btn btn-primary hideBtn";
    onMounted(() => {
        store.loadForms();
        if (template.value) {
            if (template.value.id?.toString() !== Guid.EMPTY){
                router.push(`/edit-entity-template/${template.value.id}`)
                btnClasses="btn btn-primary hideBtn";
            }
        }
    });

</script>

<template>
    <h3>Entity Template Builder</h3>
    <div class="control">
        <button :class="btnClasses" @click="createTemplate">New Template</button>
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
                    <div v-for="frm in template.entityTemplateSettings.metadataForms" :key="frm.formId">
                        <FormEntryTemplate :model="frm" class="form-field-border form-field blue" />
                    </div>
                </draggable>
                <button class="btn btn-primary btn-blue" @click="addMetadataForm">+ Add</button>
            </div>
        </div>
        <div class="form-field-border red">
            <h5>Data Forms</h5>
            <draggable class="dragArea list-group w-full" :list="template.entityTemplateSettings.dataForms">
                <div v-for="frm in template.entityTemplateSettings.dataForms" :key="frm.formId">
                    <FormEntryTemplate :model="frm" class="form-field-border form-field red" />
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
                    <FormFieldSelectionDropdown :model="titleField" :option-source="formFieldSelectorSource" :forms="template.forms" />
                </div>
            </div>
            <div class="row">
                <div class="col-2">
                    Description
                </div>
                <div class="col-10">
                    <FormFieldSelectionDropdown :model="descriptionField" :option-source="formFieldSelectorSource" :forms="template.forms" />
                </div>
            </div>
      </div>
    </div>
    <div class="alert alert-info" style="margin-top:2em;">{{template}}</div>

</template>


<style scoped src="./style.css"></style>
