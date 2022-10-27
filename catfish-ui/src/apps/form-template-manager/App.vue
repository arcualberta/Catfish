<script lang="ts" setup>
import { AppletAttribute } from '@/components/shared/props';
import { computed } from 'vue';
import {default as CrudObjectManager} from '../shared/crud-object-manager/App.vue'

import {FormBuilder} from '../../components'
import { useRoute } from 'vue-router';

import { Guid } from 'guid-typescript';

//const apiRoot = "/forms";
const props = defineProps<{
    dataAttributes?: AppletAttribute | null,
}>()

const apiRoot = computed(() => (props.dataAttributes ? props.dataAttributes["RepositoryMicroserviceUrl"] : "") + "/api/forms");

   // const route = useRoute();
   // const formId = route.params.id as unknown as Guid;

</script>

<template>
    <CrudObjectManager :api-root="apiRoot">
        <template #object-type>Form Template</template>      
       <!-- <template #list-entry-delegate>List Entry</template>-->
        <template #create-delegate><form-builder :api-root="apiRoot" /></template>
        <template #read-delegate>ReadFormComponent</template>
        <template #update-delegate>UpdateFormComponent</template>
        <template #delete-delegate>Delete</template>
    </CrudObjectManager>
</template>
