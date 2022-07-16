<script setup lang="ts">
    import { useRouter } from 'vue-router'

    const router = useRouter();
    const navigateTo = (path) => { router.push(path); }
</script>

<template>
    <h2 class="component-name">Components</h2>
    <button @click="navigateTo('/form-builder')">Form Builder</button>
</template>