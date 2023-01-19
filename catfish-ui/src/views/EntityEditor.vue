<script setup lang="ts">
// This starter template is using Vue 3 <script setup> SFCs
// Check out https://vuejs.org/api/sfc-script-setup.html#script-setup

    import { getActivePinia } from 'pinia'
    import { useRoute } from 'vue-router'
   import {default as config} from "@/appsettings";

    import { EntityEditor } from  '../components'

    const route = useRoute()
     const apiRoot= config.dataRepositoryApiRoot + "/api/entities";


</script>

<template setup>
     <EntityEditor :api-root="apiRoot" />
</template>
