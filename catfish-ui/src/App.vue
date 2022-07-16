<script setup lang="ts">
// This starter template is using Vue 3 <script setup> SFCs
// Check out https://vuejs.org/api/sfc-script-setup.html#script-setup

</script>

<template>
    <div>
        <nav class="navigationalmenu">
            <router-link to="/" class="navigation-menu-box">Home</router-link> | 
            <router-link to="/form-builder" class="navigation-menu-box">Form Builder</router-link>
        </nav>
    </div>

    <router-view />
</template>

