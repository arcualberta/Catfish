<script setup lang="ts">
// This starter template is using Vue 3 <script setup> SFCs
    // Check out https://vuejs.org/api/sfc-script-setup.html#script-setup
    import { computed } from '@vue/runtime-core';
import { useRouter } from 'vue-router'
    import { useLoginStore } from './components/login/store';

    const router = useRouter();

    const authorizationStore = useLoginStore();
    const logout = () => {
        authorizationStore.loginResult = null;
        authorizationStore.jwtToken = null;
        router.push("/");
    }

    const jwtToken = computed(()=>authorizationStore.jwtToken )
</script>

<template>
    <div class="header">
        <router-link to="/" class="navigation-menu-box">Home</router-link> | 
        <span v-if="authorizationStore?.loginResult?.success" class="user-info">
            <span class="welcome">Welcome {{authorizationStore.loginResult.name}}! </span>
            <a @click="logout" class="navigation-menu-box logout">Logout</a>
        </span>
        <router-link v-else to="/login" class="navigation-menu-box">Login</router-link>
    </div>
    <router-view />

    <div>{{jwtToken}}</div>
</template>

<style scoped>
    .logout:hover{
        cursor:pointer;
    }
    .welcome{
        color:white;
        font-weight:bold;
    }
</style>