<script lang="ts" setup>
import { AppletAttribute } from '@/components/shared/props';
import { computed, onMounted } from 'vue';
import {useCRUDManagerStore} from './store'

const props = defineProps<{
    apiRoot?: string
   
}>()
const store = useCRUDManagerStore();
const apiUrl = computed(()=>props.apiRoot);
 onMounted(() => {
      store.loadEntries(apiUrl?.value as string);
    });

</script>

<template>
    <div class="m-2">
        <div class="header">
            <router-link to="/" class="navigation-menu-box">List</router-link> | 
            <router-link to="/create" class="navigation-menu-box">Create</router-link> | 
            <router-link to="/read/381449d3-9e3d-412a-9630-ea4cb6f35d8b" class="navigation-menu-box">Read</router-link> | 
            <router-link to="/update/381449d3-9e3d-412a-9630-ea4cb6f35d8b" class="navigation-menu-box">Update</router-link> | 
            <router-link to="/delete/381449d3-9e3d-412a-9630-ea4cb6f35d8b" class="navigation-menu-box">Delete</router-link>
        </div>

        <router-view v-slot="{ Component, route }">
            <component :is="Component">
                <template #object-type><slot name="object-type"/></template>      
                <template #list-entry-delegate><slot name="list-entry-delegate"/></template>
                <template #create-delegate><slot name="create-delegate"/></template>
                <template #read-delegate><slot name="read-delegate"/></template>
                <template #udapte-delegate><slot name="udapte-delegate"/></template>
                <template #delete-delegate><slot name="delete-delegate"/></template>
            </component>
        </router-view>   
        
        <div>API Root: {{apiRoot}}</div>
    </div>
</template>