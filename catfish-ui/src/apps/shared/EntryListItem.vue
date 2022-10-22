<script lang="ts" setup>
import { Guid } from 'guid-typescript';

const props = defineProps<{
    id: Guid
}>()

</script>

<template>
    <div class="col-8">
        <slot></slot>>
    </div>
    <div class="col-4">
        <router-link :to="`/view/${id}`" class="navigation-menu-box">View</router-link>
        <router-link :to="`/edit/${id}`" class="navigation-menu-box">Edit</router-link>
    </div>
</template>