<script lang="ts" setup>
import { computed, toRef } from 'vue';
import {default as CrudObjectManager} from '../shared/crud-object-manager/App.vue'
import {WorkflowBuilder} from "../../components"
import { AppletAttribute } from '@/components/shared/props';

const props = defineProps<{
    dataAttributes?: AppletAttribute | null,
}>()

const apiRoot = computed(() => (props.dataAttributes ? props.dataAttributes["RepositoryMicroserviceUrl"] : "") + "/api/workflow");

const _dataAttributes = toRef(props, 'dataAttributes')
const userJwtToken = _dataAttributes && _dataAttributes?.value? (_dataAttributes.value["UserJwtToken"] as string) : null;
 
</script>

<template>
    <CrudObjectManager :api-root="apiRoot" :jwt-token="userJwtToken">
        <template #object-type>Workflow</template>      
       <!-- <template #list-entry-delegate>List Entry</template> -->
        <template #create-delegate><WorkflowBuilder :api-root="apiRoot" :jwt-token="userJwtToken" /></template>
        <template #read-delegate>ReadWorkflowComponent</template>
        <template #update-delegate><WorkflowBuilder :api-root="apiRoot" :jwt-token="userJwtToken" /></template>
        <template #delete-delegate>Delete</template>
    </CrudObjectManager>
</template>