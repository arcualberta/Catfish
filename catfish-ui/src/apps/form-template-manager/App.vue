<script lang="ts" setup>
import { AppletAttribute } from '@/components/shared/props';
import { computed, toRef } from 'vue';
import {default as CrudObjectManager} from '../shared/crud-object-manager/App.vue'

import {FormBuilder} from '../../components'
import { useRoute } from 'vue-router';

import { Guid } from 'guid-typescript';

//const apiRoot = "/forms";
const props = defineProps<{
    dataAttributes?: AppletAttribute | null,
}>()

const apiRoot = computed(() => (props.dataAttributes ? props.dataAttributes["RepositoryMicroserviceUrl"] : "") + "/api/forms");
const _dataAttributes = toRef(props, 'dataAttributes')
const userJwtToken = _dataAttributes && _dataAttributes?.value? (_dataAttributes.value["UserJwtToken"] as string) : null;
    

</script>

<template>
   
    <CrudObjectManager :api-root="apiRoot" :jwt-token="userJwtToken">
        <template #object-type>Form Template</template>      
       <!-- <template #list-entry-delegate>List Entry</template>-->
        <template #create-delegate><form-builder :api-root="apiRoot" :jwt-token="userJwtToken" /></template>
        <template #read-delegate>ReadFormComponent</template>
        <template #update-delegate><form-builder :api-root="apiRoot" :jwt-token="userJwtToken" /></template>
        <template #delete-delegate>Delete</template>
    </CrudObjectManager>
    <div>{{userJwtToken}}</div>
</template>
