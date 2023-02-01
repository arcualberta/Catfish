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

    const {jwtToken, loginResult} = storeToRefs(authorizationStore);
    const logout = () => {
       
        localStorage.removeItem("catfishLoginResult")
       //authorizationStore.loginResult.set({} as LoginResult)
      
       localStorage.removeItem("catfishJwtToken")
       //authorizationStore.jwtToken.set("")
        router.push("/");
       // window.location.;
    }

   
   // const jwtToken = computed(()=> authorizationStore.jwtToken.get())
    
    console.log("localstorage jwt: " + jwtToken.value.get())

   const loginRes =computed(()=> authorizationStore.loginResult.get())
    
    console.log("localstorage loginRes: " + JSON.stringify(loginResult.value.get()))
   
   console.log("sucess: " + loginRes.value?.success)
  
   watch(() => loginRes.value, async newResult => {
       // console.log('watch(() => authorizationStore.loginResult.get(), async newResult => ')
        //loginRes.value = authorizationStore.loginResult.get();
        if(newResult?.success){
            console.log("sucess: " + newResult?.success)
            router.push("/");
        }
    });

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