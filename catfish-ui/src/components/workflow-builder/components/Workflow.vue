<script setup lang="ts">
    import { Workflow } from '../models/'

    import { default as WorkflowAction } from './WorkflowAction.vue'

    defineProps < { model: Workflow } > ();

</script>

<template>
    <WorkflowAction v-for="action in model.actions" :key="action.id" :model="action" />
</template>