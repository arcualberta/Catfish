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
            <router-link to="/create" class="navigation-menu-box">Create</router-link>  
        </div>

        <router-view v-slot="{ Component, route }">
            <component :is="Component">
                <template #object-type><slot name="object-type"/></template>      
                <template #list-entry-delegate><slot name="list-entry-delegate"/></template>
                <template #create-delegate><slot name="create-delegate"/></template>
                <template #read-delegate><slot name="read-delegate"/></template>
                <template #update-delegate><slot name="update-delegate"/></template>
                <template #delete-delegate><slot name="delete-delegate"/></template>
            </component>
        </router-view>   
        
        <!--<div>API Root: {{apiRoot}}</div> -->
    </div>
</template>