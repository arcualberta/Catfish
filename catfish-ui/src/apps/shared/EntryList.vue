<script lang="ts" setup>
import { ListEntry } from '@/components/shared';
import EntryListItem from './EntryListItem.vue';


const props = defineProps<{
    crudApiRoot: string
}>()

</script>

<template>
    <!--
    <div class="row" v-for="entry in entries" :key="entry.id.toString()">
        <EntryListItem :id="entry.id">{{entry.name}}</EntryListItem>
    </div>
    -->
</template>