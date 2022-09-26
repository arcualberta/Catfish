<template>
 <h5>Entity </h5>
 <div>
   <div class="row">
        <div class="col-sm-2">
            <label >EntityType:</label>
        </div>
        <div class="col-sm-10">
           <select v-model="model.entityType"  class="form-select">
                <option v-for="type in eEntityTypes" :key="type" :value="type">{{type}}</option>
            </select> 
        </div>
    </div>
    <div class="row">
        <div class="col-sm-2">
            <label >Template:</label>
        </div>
        <div class="col-sm-10">
           <select v-model="model.templateId"  class="form-select" @change="loadTemplate()">
                <option v-for="template in templateEntries" :key="template.templateId" :value="template.templateId">{{template.templateName}}</option>
            </select> 
        </div>
    </div>
 </div>
</template>

<script setup lang="ts">

import { Pinia } from 'pinia'
import{computed} from "vue"

import {useEntityEditorStore} from "../store"
import {eEntityType} from "../../shared/constants"
import { Entity } from '../models';
import { Guid } from 'guid-typescript';
  const props = defineProps<{
       // dataAttributes?: AppletAttribute | null,
        //queryParameters?: AppletAttribute | null,
        piniaInstance: Pinia,
        model: Entity
    }>();

    const store = useEntityEditorStore(props.piniaInstance);
    //store.loadTemplates();
    const templateEntries = computed(()=>store.templates);

    const eEntityTypes = Object.values(eEntityType);

    const loadTemplate = ()=>{
        store.loadTemplate(props.model.templateId); 
        const entityTemplate=store.getEntityTemplate();
    console.log(JSON.stringify(entityTemplate)) 
    }
   
     
</script>
