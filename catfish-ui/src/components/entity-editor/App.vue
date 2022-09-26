<script setup lang="ts">
    import { Pinia } from 'pinia'
    import { computed, onMounted, ref} from 'vue'
    import { useEntityEditorStore } from './store';
    import {default as EntitySummaryEditor} from './components/entity-summary-editor.vue'
    

    const props = defineProps<{
       // dataAttributes?: AppletAttribute | null,
        //queryParameters?: AppletAttribute | null,
        piniaInstance: Pinia
    }>();

    const store = useEntityEditorStore(props.piniaInstance);
    //store.loadTemplates();
    

    onMounted(() => {
        store.loadTemplates();
       
    });

    const templateEntries = computed(()=>store.templates);
    let isShowEditor= ref(false);
    const showEditor = ()=>{
        isShowEditor.value = true;
        store.initializeEntity();
    };

    const entity = computed(()=>store.entity)
</script>

<template>
    <h3>Entity Editor</h3>
    <div class="control">
        <button @click="showEditor()">New Entity</button>
        <button class="btn btn-success">Save</button>
    </div>
    <EntitySummaryEditor v-if="isShowEditor" :model="entity" />
</template>


<style scoped src="./style.css"></style>
