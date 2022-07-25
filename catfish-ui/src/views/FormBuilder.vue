<script setup lang="ts">
// This starter template is using Vue 3 <script setup> SFCs
// Check out https://vuejs.org/api/sfc-script-setup.html#script-setup

    import { getActivePinia } from 'pinia'
    import { useRoute } from 'vue-router'
    import { Guid } from 'guid-typescript'

    import { FormBuilder } from '../components'

    const route = useRoute()
    const formId = route.params.id as unknown as Guid

</script>

<template>
    <FormBuilder :pinia-instance="getActivePinia()" repository-root="https://localhost:5020/" :form-id="formId" />
</template>
