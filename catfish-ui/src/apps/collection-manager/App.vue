<script lang="ts" setup>
import {default as CrudObjectManager} from '../shared/crud-object-manager/App.vue'

const apiRoot = "/collections";

</script>

<template>
    <CrudObjectManager :api-root="apiRoot">
        <template #object-type>Collection</template>      
        <template #list-entry-delegate>List Entry</template>
        <template #create-delegate>CreateCollectionComponent</template>
        <template #read-delegate>ReadCollectionComponent</template>
        <template #udapte-delegate>UpdateCollectionComponent</template>
        <template #delete-delegate>Delete</template>
    </CrudObjectManager>
</template>