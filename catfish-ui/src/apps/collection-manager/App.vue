<script lang="ts" setup>
import {default as CrudObjectManager} from '../shared/crud-object-manager/App.vue'

const apiRoot = "/collections";

</script>

<template>
    <CrudObjectManager :api-root="apiRoot">
        <template #object-type>Collection</template>      
        <template #list-entry-delegate>List Entry</template>
        <template #create-delegate>Create</template>
        <template #read-delegate>Read</template>
        <template #udapte-delegate>Update</template>
        <template #delete-delegate>Delete</template>
    </CrudObjectManager>
</template>