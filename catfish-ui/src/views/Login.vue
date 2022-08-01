<script setup lang="ts">
// This starter template is using Vue 3 <script setup> SFCs
// Check out https://vuejs.org/api/sfc-script-setup.html#script-setup

    import { getActivePinia } from 'pinia'
    import { Login } from '../components'

</script>

<template>
    <Login :pinia-instance="getActivePinia()" authorization-root="https://localhost:5010/" />
</template>
