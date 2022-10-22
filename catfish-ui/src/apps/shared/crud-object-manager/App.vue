<script lang="ts" setup>
import { AppletAttribute } from '@/components/shared/props';

const props = defineProps<{
    apiRoot?: string,
}>()

</script>

<template>
    <router-view v-slot="{ Component, route }">
        <component :is="Component">
            <template #object-type><slot name="object-type"/></template>
            <template #list-title><slot name="list-title"/></template>
            <template #create-title><slot name="create-title"/></template>
            <template #edit-title><slot name="edit-title"/></template>
        </component>
    </router-view>   
    
    <div class="header">
        <router-link to="/" class="navigation-menu-box">List</router-link> | 
        <router-link to="/create" class="navigation-menu-box">Create</router-link> | 
        <router-link to="/edit/381449d3-9e3d-412a-9630-ea4cb6f35d8b" class="navigation-menu-box">Edit</router-link>
    </div>
    {{apiRoot}}
    
</template>