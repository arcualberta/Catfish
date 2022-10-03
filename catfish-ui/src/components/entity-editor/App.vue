<script setup lang="ts">
    import { Pinia } from 'pinia'
    import { computed, onMounted, ref} from 'vue'
    import { useEntityEditorStore } from './store';
    import {default as EntitySummaryEditor} from './components/entity-summary-editor.vue'
    import { default as FormList } from './components/FormList.vue'
    

    const props = defineProps<{
       // dataAttributes?: AppletAttribute | null,
        //queryParameters?: AppletAttribute | null,
        piniaInstance: Pinia
    }>();

    const store = useEntityEditorStore(props.piniaInstance);
    const entityTemplate =  computed(() => store.entityTemplate);
    let selectedButton = ref("summary");

    onMounted(() => {
        store.loadTemplates();
       
    });

    const templateEntries = computed(()=>store.templates);
    let isShowEditor= ref(false);
    const createEntity = ()=>{
        store.createNewEntity();
    };
    const entity = computed(() => store.entity)

    const metadataForms = computed(() => store.entityTemplate?.entityTemplateSettings.metadataForms)
    const dataForms = computed(() => store.entityTemplate?.entityTemplateSettings.dataForms)

    const saveEntity= ()=>{store.saveEntity()}
</script>

<template>
    <h3>Entity Editor</h3>
    <div class="control">
        <button @click="createEntity()">New Entity</button>
        <button class="btn btn-success" @click="saveEntity()">Save</button>
    </div>
    <div class="form-field-border">
        <b-row v-if="entityTemplate">
            <b-col class="btn-group" role="group" id="toolBtns">
                <button class="pannel-buttons" @click="selectedButton='summary'" :class="{active: selectedButton === 'summary'}">Summary</button>
                <button class="pannel-buttons" @click="selectedButton='data'" :class="{active: selectedButton === 'data'}">Data</button>
                <button class="pannel-buttons" @click="selectedButton='metadata'" :class="{active: selectedButton === 'metadata'}">Metadata</button>
                <button class="pannel-buttons" @click="selectedButton='collections'" :class="{active: selectedButton === 'collections'}">Collection(s)</button>
                <button class="pannel-buttons" @click="selectedButton='related'" :class="{active: selectedButton === 'related'}">Related</button>
            </b-col>
        </b-row>
        <!--<div v-if="entityTemplate" class="row mt-2 pt-2 border-top">
            <div class="col-sm-10"></div>
        </div>-->
        <div v-if="selectedButton === 'summary'">
            <EntitySummaryEditor v-if="entity !== null" />Summary
        </div>
        <div v-if="selectedButton === 'data'">
            <FormList :form-entries="dataForms" :entity="entity"></FormList>
        </div>
        <div v-if="selectedButton === 'metadata'">
            <FormList :form-entries="metadataForms" :entity="entity"></FormList>
        </div>
        <div v-if="selectedButton === 'collections'">
            Collections
        </div>
        <div v-if="selectedButton === 'related'">
            Related
        </div>
    </div>
</template>


<style scoped src="./style.css"></style>
