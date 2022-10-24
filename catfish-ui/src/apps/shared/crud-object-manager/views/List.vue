<script lang="ts" setup>

const entries = [1,2,3,4,5];
</script>

<template>
    <h4>List <slot name="object-type"/>s</h4>

    <div v-for="entry in entries">
        <slot name="list-entry-delegate"></slot> {{entry}}
    </div>
</template>