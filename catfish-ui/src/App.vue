<script setup lang="ts">
// This starter template is using Vue 3 <script setup> SFCs
    // Check out https://vuejs.org/api/sfc-script-setup.html#script-setup
    import { computed, onMounted } from 'vue';
    import { useRouter } from 'vue-router'
    import { useLoginStore } from './components/login/store';
    import jwt_decode from "jwt-decode";
    const router = useRouter();

    const authorizationStore = useLoginStore();
    const logout = () => {
        authorizationStore.loginResult = null;
        localStorage.removeItem("catfishLoginResult")
       
       //localStorage.setItem("catfishJwtToken", "");
       localStorage.removeItem("catfishJwtToken")
        router.push("/");
    }

   
    const jwtToken = computed(()=> authorizationStore.jwtToken)
    //localStorage.getItem("catfishJwtToken")
    console.log("localstorage jwt: " + jwtToken.value)
   
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