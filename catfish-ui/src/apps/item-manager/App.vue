<script lang="ts" setup>
import { computed } from 'vue';
import {default as CrudObjectManager} from '../shared/crud-object-manager/App.vue'
import {EntityEditor} from "../../components"
//const apiRoot = "/items";
const props = defineProps<{
    dataAttributes?: AppletAttribute | null,
}>()

const apiRoot = computed(() => (props.dataAttributes ? props.dataAttributes["RepositoryMicroserviceUrl"] : "") + "/api/items");


</script>

<template>
    <CrudObjectManager :api-root="apiRoot">
        <template #object-type>Item</template>      
       <!-- <template #list-entry-delegate>List Entry</template> -->
        <template #create-delegate><EntityEditor :api-root="apiRoot" /></template>
        <template #read-delegate>ReadItemComponent</template>
        <template #update-delegate><EntityEditor :api-root="apiRoot" /></template>
        <template #delete-delegate>Delete</template>
    </CrudObjectManager>
</template>