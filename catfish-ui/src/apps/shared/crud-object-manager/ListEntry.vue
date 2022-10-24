<script lang="ts" setup>

import { ListEntry } from '@/components/shared';
import { computed } from 'vue';
import { default as config } from "@/appsettings";

const props = defineProps<{
    entry: ListEntry 
}>()

//API ROOT/read/{entry.id}
//
const detailUrl ="/read/" + props.entry.id;
const updateUrl ="/update/" + props.entry.id;
const deleteUrl = "/delete/" + props.entry.id;
</script>

<template>
    <div class="entryRow">
    <router-link :to="detailUrl" class="col-8">{{entry.name}}</router-link>
    <router-link :to="updateUrl" class="col-2">Update</router-link>
   <router-link :to="deleteUrl" class="col-2">Delete</router-link>
   </div>
   
</template>
<style scoped>
   .entryRow{
    border: 1px solid lightgray;
    padding: 7px;
   }
</style>