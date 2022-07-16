<script setup lang="ts">
// This starter template is using Vue 3 <script setup> SFCs
// Check out https://vuejs.org/api/sfc-script-setup.html#script-setup

    import { getActivePinia } from 'pinia'

    import { FormEditor } from '../components'
</script>

<template>
    <FormEditor :pinia-instance="getActivePinia()" repository-root="https://localhost:5020/" />
</template>
