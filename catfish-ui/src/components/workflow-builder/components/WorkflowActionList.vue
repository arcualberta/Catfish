<script setup lang="ts">
    import { useWorkflowBuilderStore } from '../store';
    import { ref } from 'vue';
    import {default as AddAction} from './AddAction.vue'
    import { WorkflowAction } from '../models';
    const store = useWorkflowBuilderStore();
    const editMode = ref(false);
    const actionId = ref("");
    const Toggle = () => {
        store.showActionPanel = true;
    }
</script>

<template>
    <div class="header-style">Actions <font-awesome-icon icon="fa-solid fa-circle-plus" style="color:#1ca5b8" @click="Toggle()"/></div>
    <div v-if="store.showActionPanel">
        <add-action  :editMode="editMode" :editActionId="actionId"/>
    </div>
</template>

