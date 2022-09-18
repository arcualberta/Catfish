<template>
    <h3>Entity Template Builder</h3>
    <button @click="createTemplate">New Template</button>

    <div v-if="template">
        <div>Name : {{template.name}} </div>
        <div>Description : {{template.description}} </div>
        <div>State: {{template.state}}</div>
       <div v-if="template.metadataForms">
             <h5>Metadata Forms</h5>
             <div v-for="frm in template.metadataForms" :key="frm.formId" >
                    <FormEntry :model="frm"  />
               </div>
       </div>
       <div v-if="template.dataForms">
             <h5>Data Forms</h5>
             <div v-for="frm in template.dataForms" :key="frm.formId" >
               <FormEntry :model="frm" />   
               </div>
        </div>
    </div>

</template>

<script setup lang="ts">
import { Pinia } from 'pinia'
import { computed } from 'vue'
import config from '../../appsettings'
import { useEntityTemplateBuilderStore } from './store';
import { default as FormEntry } from './components/FormEntry.vue';

const props = defineProps < {
        piniaInstance: Pinia,
        
     } > ();
const store = useEntityTemplateBuilderStore(props.piniaInstance);
const createTemplate = ()=> store.newTemplate();

const template = computed(()=>store.template);

</script>
<style scoped src="./style.css"></style>
