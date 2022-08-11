<script setup lang="ts">
    import { useRouter } from 'vue-router'

    const router = useRouter();
    const navigateTo = (path) => { router.push(path); }
</script>

<template>
    <h2 class="component-name">Components</h2>
    <button class="home-link" @click="navigateTo('/sandbox')">Sandbox</button>
    <button class="home-link" @click="navigateTo('/form-builder')">Form Builder</button>
    <button class="home-link" @click="navigateTo('/workflow-builder')">Workflow Builder</button>
    <button class="home-link" @click="navigateTo('/new-form-submission')">New Form Submission</button>
    <button class="home-link" @click="navigateTo('/edit-form-submission')">Edit Form Submission</button>
</template>

<style scoped>
    .home-link{
        margin:5px;
    }
</style>