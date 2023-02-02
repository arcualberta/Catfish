<script lang="ts" setup>
import { computed, toRef } from 'vue';
import {default as CrudObjectManager} from '../shared/crud-object-manager/App.vue'
import {EntityEditor} from "../../components"

const props = defineProps<{
    dataAttributes?: AppletAttribute | null,
}>()

const apiRoot = computed(() => (props.dataAttributes ? props.dataAttributes["RepositoryMicroserviceUrl"] : "") + "/api/collections");
const _dataAttributes = toRef(props, 'dataAttributes')
const userJwtToken = _dataAttributes && _dataAttributes?.value? (_dataAttributes.value["UserJwtToken"] as string) : null;
    
//console.log("user token " + userJwtToken)
</script>

<template>
    <CrudObjectManager :api-root="apiRoot" :jwt-token="userJwtToken">
        <template #object-type>Collection</template>      
        
        <template #create-delegate><entity-editor :api-root="apiRoot" :jwt-token="userJwtToken" /></template>
        <template #read-delegate>ReadCollectionComponent</template>
        <template #update-delegate><entity-editor :api-root="apiRoot" :jwt-token="userJwtToken" /></template>
        <template #delete-delegate>Delete</template>
    </CrudObjectManager>
</template>