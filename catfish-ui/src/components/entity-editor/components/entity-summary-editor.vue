<script setup lang="ts">
    import { computed, ref, watch } from "vue"
    import { storeToRefs } from "pinia"
    import { useEntityEditorStore } from "../store"
    import { eEntityType } from "../../shared/constants"
    import { Guid } from 'guid-typescript';
    import { FieldEntry } from '../../shared/form-models'
    import { EntityTemplate } from '../../entity-template-builder/models'
    import { instantiateRequiredForms, getField, getFieldData } from '@/components/shared/entity-helpers'
    import { EntityData } from "../../entity-editor/models";
    import { default as ConfirmPopUp } from '../../shared/components/pop-up/ConfirmPopUp.vue';

    import { default as FieldComponent } from '../../form-submission/components/Field.vue'
    import * as formHelper from '../../shared/form-helpers'
   
   const popupTrigger = ref(false);
    const store = useEntityEditorStore();
    //const entity = computed(() => store.entity)
    const { entity } = storeToRefs(store);

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
    //console.log("title field: " + titleField.value)
    const titleFieldData = computed(() => getFieldData(entity.value as EntityData, entityTemplate.value?.entityTemplateSettings.titleField as FieldEntry));
    // console.log("title field data: " + titleFieldData.value)
    const descriptionField = computed(() => getField(entityTemplate.value as EntityTemplate, entityTemplate.value?.entityTemplateSettings.descriptionField as FieldEntry));
   // console.log("description field: " + descriptionField.value)
    const descriptionFieldData = computed(() => getFieldData(entity.value as EntityData, entityTemplate.value?.entityTemplateSettings.descriptionField as FieldEntry));
   // console.log("description field data: " + descriptionFieldData.value)
    
    const mediaField = computed(() => getField(entityTemplate.value as EntityTemplate, entityTemplate.value?.entityTemplateSettings.mediaField as FieldEntry));
    const mediaFieldData = computed(() => getFieldData(entity.value as EntityData, entityTemplate.value?.entityTemplateSettings.mediaField as FieldEntry));
    const TogglePopup = () => (popupTrigger.value = !popupTrigger.value);


    watch(() => entityTemplate.value, async newTemplate => {
        instantiateRequiredForms(entity.value as EntityData, newTemplate as EntityTemplate);
    })
    
</script>

<template>
    <div class="pt-2 mt-2">
        <div class="row">
            <fieldset class="col-sm-7">
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
                            <option v-for="template in templateEntries" :key="template.id.toString()" :value="template.id?.toString()">{{template.name}}</option>
                        </select>
                        <span v-else>{{entityTemplate?.name}}</span>
                    </div>
                </div>
                
                <!-- <div class="row mt-2">
                     <div class="col-sm-2">
                         <label>Entity Title:</label>
                     </div>
                     <div class="col-sm-10">
                        <input type="text" v-model="entity!.title" class="form-control"  />
                     </div>
                 </div>
                 <div class="row mt-2">
                     <div class="col-sm-2">
                         <label>Entity Description:</label>
                     </div>
                     <div class="col-sm-10">
                        <input type="text" v-model="entity!.description" class="form-control"  />
                     </div>
                 </div>-->
                <br />
                <h5>Form Fields</h5>
                <FieldComponent :model="titleField" :model-data="titleFieldData" v-if="store.titleField" />
                <FieldComponent :model="descriptionField" :model-data="descriptionFieldData" v-if="store.descriptionField" />
                <FieldComponent :model="mediaField" :model-data="mediaFieldData" v-if="store.mediaField" />
                <div v-if="store.mediaField && mediaFieldData?.fileReferences?.length > 0">
                    <div v-for="fr in mediaFieldData?.fileReferences" :key="fr.id">
                        <div>{{fr.originalFileName}}</div>
                    </div>
                </div>
            </fieldset>
            <fieldset class="col-sm-5">
                <legend> Right side </legend>
                <div class="col-sm-8">
                    <div>{{titleField?.title?.values[0]?.value}}: {{titleFieldData?.multilingualTextValues[0]?.values[0]?.value}}</div>
                    <div>{{descriptionField?.title?.values[0]?.value}}: {{descriptionFieldData?.multilingualTextValues? descriptionFieldData?.multilingualTextValues[0]?.values[0]?.value: descriptionFieldData?.monolingualTextValues[0].value}}
                         
                    </div>
                    <div>{{mediaFieldData?.fileReferences[0]?.originalFileName}}</div>

                </div>
                <div class="col-sm-4" v-if="store.mediaField">
                    <img src="#" />
                </div>
            </fieldset>
        </div>
    </div>
</template>

