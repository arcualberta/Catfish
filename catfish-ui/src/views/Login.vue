<script setup lang="ts">
// This starter template is using Vue 3 <script setup> SFCs
// Check out https://vuejs.org/api/sfc-script-setup.html#script-setup
    import { watch, computed } from 'vue'
    import { getActivePinia } from 'pinia'
    import { useRouter } from 'vue-router'
    import {default as config} from '@/appsettings'
    import { Login } from '../components'
    import { useLoginStore } from '../components/login/store';

    const authorizationStore = useLoginStore();
    const router = useRouter();

    const loginRes = computed(()=> authorizationStore.loginResult.get());
    
    watch(() => loginRes.value, async newResult => {
       // console.log('watch(() => authorizationStore.loginResult.get(), async newResult => ')
        if(newResult?.success){
            router.push("/");
        }
    });

    const apiRoot = config.authorizationApiRoot;

</script>

<template>
    <Login  :authorization-root="apiRoot" />
</template>
