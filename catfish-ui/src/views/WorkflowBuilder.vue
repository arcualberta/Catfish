<script setup lang="ts">
    import { getActivePinia } from 'pinia'
    import { useRoute } from 'vue-router'
    import { Guid } from 'guid-typescript'

    import { WorkflowBuilder } from '../components'

    const route = useRoute()
    const workflowId = route.params.id as unknown as Guid

</script>

<template>
    <WorkflowBuilder :pinia-instance="getActivePinia()" repository-root="https://localhost:5020/" :workflow-id="workflowId" />
</template>
