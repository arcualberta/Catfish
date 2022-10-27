<script lang="ts" setup>
import { AppletAttribute } from '@/components/shared/props';
import { computed } from 'vue';
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

</script>

<template>
    <CrudObjectManager :api-root="apiRoot">
        <template #object-type>Entity Template</template>      
         <template #create-delegate><EntityTemplateBuilder :api-root="apiRoot" /></template>
        <template #read-delegate>ReadEntityTemplateComponent</template>
        <template #update-delegate><EntityTemplateBuilder :api-root="apiRoot" :template-id="templateId" /></template>
        <template #delete-delegate>Delete</template>
    </CrudObjectManager>
</template>