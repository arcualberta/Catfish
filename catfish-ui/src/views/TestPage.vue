<script setup lang="ts">
    import { Carousel } from '../components/'

    const slides = ['img-1', 'img-2'];
</script>

<template>
    <h4>Test Page</h4>
    <di class="cols-md-6">
        <Carousel :auto-play="true" :timeout-auto-play="10000" :navigation="true" :pagination="false" :slides="slides" />
    </di>
</template>