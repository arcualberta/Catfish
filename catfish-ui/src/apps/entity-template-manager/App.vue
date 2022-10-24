<script lang="ts" setup>
import { AppletAttribute } from '@/components/shared/props';
import { computed } from 'vue';
import {default as CrudObjectManager} from '../shared/crud-object-manager/App.vue'

const props = defineProps<{
    dataAttributes?: AppletAttribute | null,
}>()

const apiRoot = computed(() => (props.dataAttributes ? props.dataAttributes["RepositoryMicroserviceUrl"] : "") + "/api/entity-templates");

</script>

<template>
    <CrudObjectManager :api-root="apiRoot">
        <template #object-type>Entity Template</template>      
        <template #list-entry-delegate>List Entry</template>
        <template #create-delegate>CreateEntityTemplateComponent</template>
        <template #read-delegate>ReadEntityTemplateComponent</template>
        <template #udapte-delegate>UpdateEntityTemplateComponent</template>
        <template #delete-delegate>Delete</template>
    </CrudObjectManager>
</template>