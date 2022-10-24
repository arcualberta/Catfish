<script lang="ts" setup>
import {useCRUDManagerStore} from '../store'
import {storeToRefs} from 'pinia'

const store = useCRUDManagerStore();

const {entries} = storeToRefs(store);
//const entries = store//[1,2,3,4,5];
</script>

<template>
    <h4>List <slot name="object-type"/>s</h4>

    <div v-for="entry in entries">
        <slot name="list-entry-delegate"></slot> {{entry}}
    </div>
</template>