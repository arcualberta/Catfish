<script lang="ts" setup>
import { computed, toRef } from 'vue';
import {default as CrudObjectManager} from '../shared/crud-object-manager/App.vue'
import {EntityEditor} from "../../components"
//const apiRoot = "/items";
const props = defineProps<{
    dataAttributes?: AppletAttribute | null,
}>()

const apiRoot = computed(() => (props.dataAttributes ? props.dataAttributes["RepositoryMicroserviceUrl"] : "") + "/api/items");


const _dataAttributes = toRef(props, 'dataAttributes')
const userJwtToken = _dataAttributes && _dataAttributes?.value? (_dataAttributes.value["UserJwtToken"] as string) : null;
  
</script>

<template>
    <CrudObjectManager :api-root="apiRoot" :jwt-token="userJwtToken">
        <template #object-type>Item</template>      
       <!-- <template #list-entry-delegate>List Entry</template> -->
        <template #create-delegate><EntityEditor :api-root="apiRoot" :jwt-token="userJwtToken" /></template>
        <template #read-delegate>ReadItemComponent</template>
        <template #update-delegate><EntityEditor :api-root="apiRoot" :jwt-token="userJwtToken" /></template>
        <template #delete-delegate>Delete</template>
    </CrudObjectManager>
</template>