<script setup lang="ts">
// This starter template is using Vue 3 <script setup> SFCs
// Check out https://vuejs.org/api/sfc-script-setup.html#script-setup

    import { ref } from 'vue'
    import { getActivePinia } from 'pinia'
    import { useRoute } from 'vue-router'
    import { Guid } from 'guid-typescript'
    import { useEntityEditorStore } from '../components/entity-editor/store';
    import { EntityEditor } from '../components'
 
    const route = useRoute()
    const entityId = route.params.entityId as unknown as Guid
    const store = useEntityEditorStore(getActivePinia());
    if(entityId)
        store.loadEntity(entityId)
       

</script>

<template>
    <h5>Edit Entity </h5>
    <EntityEditor  :pinia-instance="getActivePinia()"   />
</template>


