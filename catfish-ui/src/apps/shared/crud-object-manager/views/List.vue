<script lang="ts" setup>
import {useCRUDManagerStore} from '../store'
import {storeToRefs} from 'pinia'
import {default as ListEntry} from '../ListEntry.vue'

const store = useCRUDManagerStore();

const {entries} = storeToRefs(store);
//const entries = store//[1,2,3,4,5];
</script>

<template>
    <h4>List <slot name="object-type"/>s</h4>

    <div v-for="entry in entries" :key="entry.id.toString()" >
      
       <slot name="list-entry-delegate"><component :is="ListEntry" :entry="entry" /></slot>
    </div>

</template>