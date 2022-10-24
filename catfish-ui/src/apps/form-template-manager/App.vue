<script lang="ts" setup>
import {default as CrudObjectManager} from '../shared/crud-object-manager/App.vue'

const apiRoot = "/forms";

</script>

<template>
    <CrudObjectManager :api-root="apiRoot">
        <template #object-type>Form Template</template>      
        <template #list-entry-delegate>List Entry</template>
        <template #create-delegate>CreateFormComponent</template>
        <template #read-delegate>ReadFormComponent</template>
        <template #udapte-delegate>UpdateFormComponent</template>
        <template #delete-delegate>Delete</template>
    </CrudObjectManager>
</template>