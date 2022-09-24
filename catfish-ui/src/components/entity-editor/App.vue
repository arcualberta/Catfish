<script setup lang="ts">
    import { Pinia } from 'pinia'
    import { computed, onMounted } from 'vue'
    import { useEntityEditorStore } from './store';
    

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
</script>

<template>
    <h3>Entity Editor</h3>
    <div class="control">
        <button >New Editor</button>
        <button class="btn btn-success">Save</button>
    </div>
    
</template>


<style scoped src="./style.css"></style>
