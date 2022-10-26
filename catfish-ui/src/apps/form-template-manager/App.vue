<script lang="ts" setup>
import { AppletAttribute } from '@/components/shared/props';
import { computed } from 'vue';
import {default as CrudObjectManager} from '../shared/crud-object-manager/App.vue'

//import {EntityTemplateBuilder} from '../../components'

//const apiRoot = "/forms";
const props = defineProps<{
    dataAttributes?: AppletAttribute | null,
}>()

const apiRoot = computed(() => (props.dataAttributes ? props.dataAttributes["RepositoryMicroserviceUrl"] : "") + "/api/entity-templates");


</script>

<template>
    <CrudObjectManager :api-root="apiRoot">
        <template #object-type>Form Template</template>      
       <!-- <template #list-entry-delegate>List Entry</template>-->
        <template #create-delegate>CreateFormComponent</template>
        <template #read-delegate>ReadFormComponent</template>
        <template #udapte-delegate>UpdateFormComponent</template>
        <template #delete-delegate>Delete</template>
    </CrudObjectManager>
</template>
