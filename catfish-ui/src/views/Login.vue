<script setup lang="ts">
// This starter template is using Vue 3 <script setup> SFCs
// Check out https://vuejs.org/api/sfc-script-setup.html#script-setup
    import { watch } from 'vue'
    import { getActivePinia } from 'pinia'
    import { useRouter } from 'vue-router'

    import { Login } from '../components'
    import { useAuthorizationStore } from '../components/login/store';

    const authorizationStore = useAuthorizationStore();
    const router = useRouter();

    watch(() => authorizationStore.loginResult, async newResult => {
        console.log('watch(() => authorizationStore.loginResult, async newResult => ')
        if (newResult?.success)
            router.push("/");
    })

</script>

<template>
    <Login :pinia-instance="getActivePinia()" authorization-root="https://localhost:5010/" />
</template>
