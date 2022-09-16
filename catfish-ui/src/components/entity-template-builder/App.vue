<template>
    <h3>Entity Template Builder</h3>
    <button >New Template</button>

</template>

<script setup lang="ts">
import { Pinia } from 'pinia'
import { onMounted } from 'vue'
import config from '../../appsettings'
import { useEntityTemplateBuilderStore } from './store';

const props = defineProps < {
        piniaInstance: Pinia,
        
     } > ();
const store = useEntityTemplateBuilderStore(props.piniaInstance);
//const createTemplate = store.newTemplate()
</script>
<style scoped src="./style.css"></style>
