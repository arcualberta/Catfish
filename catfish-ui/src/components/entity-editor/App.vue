<script setup lang="ts">
    import { Pinia } from 'pinia'
    //import {storeToRefs} from 'pinia'

    import { computed, onMounted, ref} from 'vue'
    import { useEntityEditorStore } from './store';
    import {default as EntitySummaryEditor} from './components/entity-summary-editor.vue'
    import { default as FormList } from './components/FormList.vue'
    import { default as AssociationPanel } from './components/EntityAssociationPanel.vue'
    import { default as TransientMessage } from '../shared/components/transient-message/TransientMessage.vue'
    import { useRoute ,useRouter } from 'vue-router';
    import { Guid } from 'guid-typescript';
    
    const props = defineProps<{
        apiRoot?: string | null,
        jwtToken?: string | null
    }>();
    const memberofValue = ref("Member of");
    const collectionValue = ref("Collections")
    const relationshipValue = ref("Relationship");
    const ItemValue = ref("Items")
    const store = useEntityEditorStore();
    
    //set apiRoot
     
    if(props.apiRoot){
        //console.log("api root from props: " + props.apiRoot)
        store.setApiRoot(props.apiRoot);
    }

    //stored the jwt Token if existed
    if(props.jwtToken && localStorage.getItem("catfishJwtToken") === null)
    {
        localStorage.setItem("catfishJwtToken", props.jwtToken);
    }

    const entityTemplate =  computed(() => store.entityTemplate);
    let selectedButton = ref("summary");
    
    const route = useRoute();
    const entityId =route.params.id as unknown as Guid; 
    onMounted(() => {

        if(entityId){
            //console.log("entity Id: " + entityId.toString())
            store.loadEntity(entityId);
        }
        else{
           // console.log("load empty template")
            store.loadTemplates();
        }
       
    });

    const templateEntries = computed(()=>store.templates);
    
    const createEntity = ()=>{
        store.createNewEntity();
    };
    let isNewEntity = ref(true);
    if(entityId){
           store.loadEntity(entityId);
           isNewEntity.value=false;
    }
    const entity = computed(() => store.entity)
   
    const metadataForms = computed(() => store.entityTemplate?.entityTemplateSettings.metadataForms)
    const dataForms = computed(() => store.entityTemplate?.entityTemplateSettings.dataForms)

    const saveEntity= ()=>{
        store.saveEntity();
        isNewEntity.value=false;
    }
    
    const files =computed(()=>store.getFiles)

     onMounted(() => {
        createEntity();
    });
</script>

<template>
    
     <TransientMessage :model="store.transientMessageModel"></TransientMessage>
    <div class="control">
       <!-- <button @click="createEntity()" v-if="isNewEntity">New Entity</button> -->
        <button class="btn btn-success" @click="saveEntity()" >Save</button>
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
            <AssociationPanel :entity="entity" :relationshipType="memberofValue" :panelTitle="collectionValue"></AssociationPanel>
        </div>
        <div v-if="selectedButton === 'related'">
            <AssociationPanel :entity="entity" :relationshipType="relationshipValue" :panelTitle="ItemValue"></AssociationPanel>
        </div>
    </div>
    <div v-for='f in files' :key='f.name'>
       {{f.name}}
    </div>
</template>


<style scoped src="./style.css"></style>
