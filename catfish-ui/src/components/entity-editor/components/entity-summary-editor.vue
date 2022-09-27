<template>
    <div class="pt-2 mt-2">
        <div class="row mt-2">
            <div class="col-sm-2">
                <label>EntityType:</label>
            </div>
            <div class="col-sm-10">
                <select v-if="isNewEntity" v-model="entity.entityType" class="form-select">
                    <option v-for="type in eEntityTypes" :key="type" :value="type">{{type}}</option>
                </select>
                <span v-else>{{entity.entityType}}</span>
            </div>
        </div>
        <div class="row mt-2">
            <div class="col-sm-2">
                <label>Template:</label>
            </div>
            <div class="col-sm-10">
                <select v-if="isNewEntity" v-model="entity.templateId" class="form-select">
                    <option v-for="template in templateEntries" :key="template.templateId" :value="template.templateId">{{template.templateName}}</option>
                </select>
                <span v-else>{{entityTemplate.name}}</span>
            </div>
        </div>
        <div class="row mt-2">
            <div class="col-sm-2">
                <label>Title:</label>
            </div>
            <div class="col-sm-10" >
               <FieldComponent :model="titleField" v-if="titleField" />
               {{titleField}}
            </div>
        </div>
        <div class="row mt-2">
            <div class='col-sm-2'>
                <label>Description:</label>
            </div>
            <div class="col-sm-10">
               
                <FieldComponent :model="descriptionField" v-if="descriptionField" />
                {{descriptionField}}
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">

    import { computed, ref, watch } from "vue"

    import { useEntityEditorStore } from "../store"
    import { eEntityType } from "../../shared/constants"
    import { Guid } from 'guid-typescript';
    import { Field, Form } from '../../shared/form-models'
    import {EntityTemplate} from '../../entity-template-builder/models'
    import {default as FieldComponent} from '../../form-submission/components/Field.vue'
   
   const store = useEntityEditorStore();
    const entity = computed(() => store.entity)
    const isNewEntity = computed(() => store.entity!.id.toString() === Guid.EMPTY);
    const templateEntries = computed(() => store.templates);
    const entityTemplate = computed(() => store.entityTemplate);
    const entityTemplateSettings = computed(() => store.entityTemplate?.entityTemplateSettings)
    const metadataFormEntries = computed(() => entityTemplate.value?.entityTemplateSettings.metadataForms)
    //const dataForms = computed(() => entityTemplate.value!.forms!.filter(form => metadataFormEntries.value!.map(formEntry => formEntry.id).findIndex((form as Form).id) > 0)
    const eEntityTypes = Object.values(eEntityType);

    const titleField= ref({}as Field);
 const descriptionField=  ref({}as Field);;

    watch(() => entity.value?.templateId, async newTemplateId => {
        store.loadTemplate(newTemplateId as Guid);
    })

    watch(()=> entityTemplate.value, async newTemplate =>{
        // get the title and description 
        console.log("the entity template : " + JSON.stringify(newTemplate))
        let frm = newTemplate?.forms?.find((form) => {return form.id === newTemplate?.entityTemplateSettings?.titleField?.formId});
        console.log("the form : " + frm)
       titleField = frm?.fields.filter((field)=>{ return field.id ==newTemplate?.entityTemplateSettings?.titleField?.fieldId});
        descriptionField = frm?.fields.filter((field)=>{ return field.id ==newTemplate?.entityTemplateSettings?.descriptionField?.fieldId});

        console.log("titleField : " + titleField)
       console.log("descriptionField : " + titleField)
    })
</script>
