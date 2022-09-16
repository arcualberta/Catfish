<template>
    <h3>Entity Template Builder</h3>
    <button @click="createTemplate">New Template</button>

    <div>{{_template}}</div>

</template>

<script setup lang="ts">
import { Pinia } from 'pinia'
import { computed } from 'vue'
import config from '../../appsettings'
import { useEntityTemplateBuilderStore } from './store';

const props = defineProps < {
        piniaInstance: Pinia,
        
     } > ();
const store = useEntityTemplateBuilderStore(props.piniaInstance);
const createTemplate = ()=> store.newTemplate();

const _template = computed(()=>store.template);

</script>
<style scoped src="./style.css"></style>
