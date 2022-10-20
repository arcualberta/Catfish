<script lang="ts" setup>
import { AppletAttribute } from '@/components/shared/props';

const props = defineProps<{
    dataAttributes?: AppletAttribute | null,
}>()

</script>

<template>
    <h4>Form Templates</h4>
    <div class="header">
        <router-link to="/" class="navigation-menu-box">List</router-link> | 
        <router-link to="/create" class="navigation-menu-box">Create</router-link> | 
        <router-link to="/edit/381449d3-9e3d-412a-9630-ea4cb6f35d8b" class="navigation-menu-box">Edit</router-link>
    </div>
    <router-view /> 
</template>