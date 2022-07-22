<script setup lang="ts">
// This starter template is using Vue 3 <script setup> SFCs
// Check out https://vuejs.org/api/sfc-script-setup.html#script-setup

    import { ref } from 'vue'
    import { getActivePinia } from 'pinia'

    import { FormSubmission } from '../components'
    const formId = ref('6e1bdee4-9694-c599-624c-7d268d50031a')

</script>

<template>
    <FormSubmission :pinia-instance="getActivePinia()" repository-root="https://localhost:5020/" :form-id="formId" />
</template>
