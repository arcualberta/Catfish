<script lang="ts" setup>

const items = [1,2,3,4,5];
</script>

<template>
    <h4>List <slot name="object-type"/>s</h4>

    <div v-for="item in items">
        <slot name="list-entry-delegate"></slot> {{item}}
    </div>
</template>