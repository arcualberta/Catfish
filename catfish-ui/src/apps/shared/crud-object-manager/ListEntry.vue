<script lang="ts" setup>

import { ListEntry } from '@/components/shared';
import { computed } from 'vue';

const props = defineProps<{
    listEntry: ListEntry
   
}>()
const listEntry = computed(()=>props.listEntry as ListEntry);
</script>

<template>
   <div>ListEntry component </div>
    <div class="col-sm-8">{listEntry.name}}</div>
</template>