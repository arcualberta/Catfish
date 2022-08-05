<script setup lang="ts">
// This starter template is using Vue 3 <script setup> SFCs
// Check out https://vuejs.org/api/sfc-script-setup.html#script-setup

    import { ref } from 'vue'
    import { getActivePinia } from 'pinia'
    import { useRoute } from 'vue-router'
    import { Guid } from 'guid-typescript'

    import { FormSubmission } from '../components'

    const route = useRoute()
    const submissionId = route.params.submissionId as unknown as Guid

</script>

<template>
    <FormSubmission v-if="submissionId" :pinia-instance="getActivePinia()" repository-root="https://localhost:5020/" :submission-id="submissionId" />
    <div v-else class="alert alert-danger mt-5">Please append the submission ID to the URL.</div>
</template>
