<script setup lang="ts">
// This starter template is using Vue 3 <script setup> SFCs
// Check out https://vuejs.org/api/sfc-script-setup.html#script-setup

    import { ref } from 'vue'
    import { getActivePinia } from 'pinia'
    import { useRoute } from 'vue-router'
    import { Guid } from 'guid-typescript'
    import {default as config} from "@/appsettings";
    import { FormSubmission } from '../components'

    const route = useRoute()
    const submissionId = route.params.submissionId as unknown as Guid
     const apiRoot= config.dataRepositoryApiRoot;


</script>

<template>
    <FormSubmission v-if="submissionId" :repository-root="apiRoot" :submission-id="submissionId" />
    <div v-else class="alert alert-danger mt-5">Please append the submission ID to the URL.</div>
</template>
