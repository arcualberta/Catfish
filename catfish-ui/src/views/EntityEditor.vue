<script setup lang="ts">
// This starter template is using Vue 3 <script setup> SFCs
// Check out https://vuejs.org/api/sfc-script-setup.html#script-setup

    import { getActivePinia } from 'pinia'
    import { useRoute } from 'vue-router'
   

    import { EntityEditor } from  '../components'

    const route = useRoute()
   

</script>

<template setup>
     <EntityEditor api-root="https://localhost:5020/api/entities" />
</template>
