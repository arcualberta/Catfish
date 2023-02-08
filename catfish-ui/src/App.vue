<script setup lang="ts">
// This starter template is using Vue 3 <script setup> SFCs
    // Check out https://vuejs.org/api/sfc-script-setup.html#script-setup
    import { computed, onMounted, ref, watch } from 'vue';
    import { useRouter } from 'vue-router'
    import { useLoginStore } from './components/login/store';
    import jwt_decode from "jwt-decode";
    import { LoginResult } from './components/login/models';

    import {storeToRefs} from 'pinia'

    const router = useRouter();

    const authorizationStore = useLoginStore();

    let loginRes = computed(() => authorizationStore.loginResult)
    let jwtToken = computed(() => authorizationStore.jwtToken);
    const logout = () => {
        authorizationStore.loginResult = {} as LoginResult
        authorizationStore.jwtToken = ""

        router.push("/");
    }

   
   // const jwtToken = computed(()=> authorizationStore.jwtToken.get())
    
    console.log("localstorage jwt: " + jwtToken.value)

   //const loginRes =computed(()=> authorizationStore.loginResult.get())
    
    console.log("localstorage loginRes: " + JSON.stringify(loginRes.value))
   
   console.log("sucess: " + loginRes.value?.success)
  
  

</script>

<template>
    <div class="header">
    {{loginRes?.success}}
        <router-link to="/" class="navigation-menu-box">Home</router-link> | 
        <span v-if="loginRes?.success" class="user-info">
            <span class="welcome">Welcome {{loginRes?.name}}! </span>
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