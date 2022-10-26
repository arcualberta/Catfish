<script lang="ts" setup>
import { computed } from 'vue';
import {default as CrudObjectManager} from '../shared/crud-object-manager/App.vue'

const props = defineProps<{
    dataAttributes?: AppletAttribute | null,
}>()

const apiRoot = computed(() => (props.dataAttributes ? props.dataAttributes["RepositoryMicroserviceUrl"] : "") + "/api/collections");


</script>

<template>
    <CrudObjectManager :api-root="apiRoot">
        <template #object-type>Collection</template>      
        
        <template #create-delegate>CreateCollectionComponent</template>
        <template #read-delegate>ReadCollectionComponent</template>
        <template #udapte-delegate>UpdateCollectionComponent</template>
        <template #delete-delegate>Delete</template>
    </CrudObjectManager>
</template>