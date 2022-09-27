<template>
    <h5>Entity </h5>
    <div>
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
    </div>
</template>

<script setup lang="ts">

    import { computed, watch } from "vue"

    import { useEntityEditorStore } from "../store"
    import { eEntityType } from "../../shared/constants"
    import { Guid } from 'guid-typescript';

    const store = useEntityEditorStore();
    const entity = computed(() => store.entity)
    const isNewEntity = computed(() => store.entity!.id.toString() === Guid.EMPTY);
    const templateEntries = computed(() => store.templates);
    const entityTemplate = computed(() => store.entityTemplate)

    const eEntityTypes = Object.values(eEntityType);

    watch(() => entity.value?.templateId, async newTemplateId => {
        store.loadTemplate(newTemplateId as Guid);
    })
</script>
