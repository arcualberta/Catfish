<script lang="ts" setup>
import {default as CrudObjectManager} from '../shared/crud-object-manager/App.vue'

const apiRoot = "/entities";

</script>

<template>
    <CrudObjectManager :api-root="apiRoot">
        <template #object-type>Item</template>      
        <template #list-title>List Items</template>
        <template #edit-title>Edit Items</template>
        <template #create-title>Create Items</template>
    </CrudObjectManager>
</template>