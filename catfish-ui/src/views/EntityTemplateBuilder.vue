<script setup lang="ts">
// This starter template is using Vue 3 <script setup> SFCs
// Check out https://vuejs.org/api/sfc-script-setup.html#script-setup

    import { getActivePinia } from 'pinia'
    import { useRoute } from 'vue-router'
   import {default as config} from "@/appsettings";

    import { EntityTemplateBuilder } from  '../components'

    const route = useRoute();
    const apiRoot= config.dataRepositoryApiRoot + "/api/entity-templates";


</script>

<template setup>
     <EntityTemplateBuilder :api-root="apiRoot" />
</template>
