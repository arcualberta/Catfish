<script lang="ts" setup>

import { ListEntry } from '@/components/shared';
import { computed } from 'vue';

const props = defineProps<{
    entry: ListEntry
   
}>()
</script>

<template>
    <div class="col-sm-8">{{entry.name}}</div>
</template>