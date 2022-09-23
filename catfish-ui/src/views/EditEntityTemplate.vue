<script setup lang="ts">
// This starter template is using Vue 3 <script setup> SFCs
// Check out https://vuejs.org/api/sfc-script-setup.html#script-setup

    import { ref } from 'vue'
    import { getActivePinia } from 'pinia'
    import { useRoute } from 'vue-router'
    import { Guid } from 'guid-typescript'
    import { useEntityTemplateBuilderStore } from '../components/entity-template-builder/store';
    import { EntityTemplateBuilder } from '../components'
 
    const route = useRoute()
    const templateId = route.params.templateId as unknown as Guid
    const store = useEntityTemplateBuilderStore(getActivePinia());
    if(templateId)
        store.loadTemplate(templateId)
       

</script>

<template>
    <h5>Edit Template </h5>
    <EntityTemplateBuilder  :pinia-instance="getActivePinia()"  />
   </template>
