<script lang="ts" setup>
import {default as CrudObjectManager} from '../shared/crud-object-manager/App.vue'

const apiRoot = "/entities";

</script>

<template>
    <CrudObjectManager :api-root="apiRoot">
        <title>Item Manager</title>
    </CrudObjectManager>
</template>