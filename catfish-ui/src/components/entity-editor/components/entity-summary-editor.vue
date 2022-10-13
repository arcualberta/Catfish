<template>
    <div class="pt-2 mt-2">
        <div class="row mt-2">
            <div class="col-sm-2">
                <label>EntityType:</label>
            </div>
            <div class="col-sm-10">
                <select v-if="isNewEntity" v-model="entity!.entityType" class="form-select">
                    <option v-for="type in eEntityTypes" :key="type" :value="type">{{type}}</option>
                </select>
                <span v-else>{{entity?.entityType}}</span>
            </div>
        </div>
        <div class="row mt-2">
            <div class="col-sm-2">
                <label>Template:</label>
            </div>
            <div class="col-sm-10">
                <select v-if="isNewEntity" v-model="entity.templateId" class="form-select">
                    <option v-for="template in templateEntries" :key="template.templateId" :value="template.templateId?.toString()">{{template.templateName}}</option>
                </select>
                <span v-else>{{entityTemplate?.name}}</span>
            </div>
        </div>
        <br />
        <FieldComponent :model="titleField" :model-data="titleFieldData" v-if="store.titleField" />
        <FieldComponent :model="descriptionField" :model-data="descriptionFieldData" v-if="store.descriptionField" />
        <FieldComponent :model="mediaField" :model-data="mediaFieldData" v-if="store.mediaField" />
        
       
    </div>
</template>

<script setup lang="ts">

    import { computed, ref, watch } from "vue"

    import { useEntityEditorStore } from "../store"
    import { eEntityType } from "../../shared/constants"
    import { Guid } from 'guid-typescript';
    import { Field, Form, FieldEntry } from '../../shared/form-models'
    import { EntityTemplate } from '../../entity-template-builder/models'
    import { instantiateRequiredForms, getField, getFieldData } from '@/components/shared/entity-helpers'
    import { EntityData } from "../../entity-editor/models";

    import { default as FieldComponent } from '../../form-submission/components/Field.vue'

    const store = useEntityEditorStore();
    const entity = computed(() => store.entity)
   
    watch(() => entity.value?.templateId, async newTemplateId => {
        store.loadTemplate(newTemplateId as Guid);
    })
   
    //if(entity.value?.templateId.toString() !== Guid.EMPTY)
        store.loadTemplate(entity.value?.templateId as Guid);
    const isNewEntity = computed(() => store.entity!.id.toString() === Guid.EMPTY);
    const templateEntries = computed(() => store.templates);
    const entityTemplate = computed(() => store.entityTemplate);
   
    const eEntityTypes = Object.values(eEntityType);

    const titleField = computed(() => getField(entityTemplate.value as EntityTemplate, entityTemplate.value?.entityTemplateSettings.titleField as FieldEntry));
    const titleFieldData = computed(() => getFieldData(entity.value as EntityData, entityTemplate.value?.entityTemplateSettings.titleField as FieldEntry));
    const descriptionField = computed(() => getField(entityTemplate.value as EntityTemplate, entityTemplate.value?.entityTemplateSettings.descriptionField as FieldEntry));
    const descriptionFieldData = computed(() => getFieldData(entity.value as EntityData, entityTemplate.value?.entityTemplateSettings.descriptionField as FieldEntry));
    const mediaField = computed(() => getField(entityTemplate.value as EntityTemplate, entityTemplate.value?.entityTemplateSettings.mediaField as FieldEntry));
    const mediaFieldData = computed(() => getFieldData(entity.value as EntityData, entityTemplate.value?.entityTemplateSettings.mediaField as FieldEntry));

   

    watch(() => entityTemplate.value, async newTemplate => {
        instantiateRequiredForms(entity.value as EntityData, newTemplate as EntityTemplate);
    })

</script>
