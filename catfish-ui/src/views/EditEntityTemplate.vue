<script setup lang="ts">
// This starter template is using Vue 3 <script setup> SFCs
// Check out https://vuejs.org/api/sfc-script-setup.html#script-setup

    import { ref } from 'vue'
    import { getActivePinia } from 'pinia'
    import { useRoute } from 'vue-router'
    import { Guid } from 'guid-typescript'
    import { useEntityTemplateBuilderStore } from '../components/entity-template-builder/store';
    import { EntityTemplateBuilder } from '../components'
    import { default as config } from "@/appsettings";

 
    const route = useRoute()
    const templateId = route.params.id as unknown as Guid
    const store = useEntityTemplateBuilderStore();
    if(templateId)
        store.loadTemplate(templateId)
       
    const apiRoot= config.dataRepositoryApiRoot + "/api/entity-templates";

</script>

<template>
    <h5>Edit Template </h5>
    <EntityTemplateBuilder  :api-root="apiRoot" />
</template>


