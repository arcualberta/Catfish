<script lang="ts" setup>
import { AppletAttribute } from '@/components/shared/props';
import { computed, toRef } from 'vue';
import {default as CrudObjectManager} from '../shared/crud-object-manager/App.vue'
import {EntityTemplateBuilder} from '../../components'
import { useRoute } from 'vue-router';
import { Guid } from 'guid-typescript';

const props = defineProps<{
    dataAttributes?: AppletAttribute | null,
}>()

const apiRoot = computed(() => (props.dataAttributes ? props.dataAttributes["RepositoryMicroserviceUrl"] : "") + "/api/entity-templates");
const route = useRoute();
const templateId = route.params.id as unknown as Guid;

const _dataAttributes = toRef(props, 'dataAttributes')
const userJwtToken = _dataAttributes && _dataAttributes?.value? (_dataAttributes.value["UserJwtToken"] as string) : null;
    
</script>

<template>
    <CrudObjectManager :api-root="apiRoot" :jwt-token="userJwtToken">
        <template #object-type>Entity Template</template>      
         <template #create-delegate><EntityTemplateBuilder :api-root="apiRoot" :jwt-token="userJwtToken" /></template>
        <template #read-delegate>ReadEntityTemplateComponent</template>
        <template #update-delegate><EntityTemplateBuilder :api-root="apiRoot" :jwt-token="userJwtToken" /></template>
        <template #delete-delegate>Delete</template>
    </CrudObjectManager>
</template>