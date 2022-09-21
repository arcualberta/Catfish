<template>
    <h3>Entity Template Builder</h3>
    <button @click="createTemplate">New Template</button>

    <div v-if="_template">
       <div>Name : {{_template.name}} </div>
       <div>Description : {{_template.description}} </div>
       <div v-if="_template.metadataForms">
             <h5>Metadata Forms</h5>
             <div v-for="frm in _template.metadataForms" :key="frm.name" >
                    <FormEntry :model="frm"  />
               </div>
       </div>
       <div v-if="_template.dataForms">
             <h5>Data Forms</h5>
             <div v-for="frm in _template.dataForms" :key="frm.name" >
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

const _template = computed(()=>store.template);

</script>
<style scoped src="./style.css"></style>
