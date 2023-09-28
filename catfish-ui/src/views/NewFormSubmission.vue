<script setup lang="ts">
// This starter template is using Vue 3 <script setup> SFCs
// Check out https://vuejs.org/api/sfc-script-setup.html#script-setup

    import {  watch } from 'vue'
    import { getActivePinia } from 'pinia'
    import { useRoute, useRouter } from 'vue-router'
    import { Guid } from 'guid-typescript'

    import { FormSubmission } from '../components'
    import { useFormSubmissionStore } from '../components'

    const route = useRoute()
    const formId = route.params.formId as unknown as Guid

    const router = useRouter();
    const formSubmissionStore = useFormSubmissionStore();
    watch(() => formSubmissionStore.formData?.id, async (newId, oldId) => {
        if ((oldId === Guid.EMPTY as unknown as Guid) && (newId !== Guid.EMPTY as unknown as Guid)) {
          //  router.push(`/edit-form-submission/${newId}`)
        }
    })
</script>

<template>
    <FormSubmission v-if="formId" :pinia-instance="getActivePinia()" repository-root="https://localhost:5020/" :form-id="formId" />
    <div v-else class="alert alert-danger mt-5">Please append the form ID to the URL.</div>
</template>
