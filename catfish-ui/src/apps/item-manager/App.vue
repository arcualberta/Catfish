<script lang="ts" setup>
import {default as CrudObjectManager} from '../shared/crud-object-manager/App.vue'

const apiRoot = "/items";

</script>

<template>
    <CrudObjectManager :api-root="apiRoot">
        <template #object-type>Item</template>      
        <template #list-entry-delegate>List Entry</template>
        <template #create-delegate>CreateItemComponent</template>
        <template #read-delegate>ReadItemComponent</template>
        <template #udapte-delegate>UpdateItemComponent</template>
        <template #delete-delegate>Delete</template>
    </CrudObjectManager>
</template>