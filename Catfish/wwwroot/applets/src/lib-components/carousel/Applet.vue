<script lang="ts">
    import { defineComponent } from 'vue';
    import props from '../shared/props'

    export default defineComponent({
        name: "Carousel",
        components: {
        },
        props,
        setup(p, ctx) {
            console.log('Carousel setup ...')
            console.log('props: ', p)
            console.log('context: ', ctx)
        },
        mounted() {
            console.log('Carousel mounted ...')
        }
    });
</script>

<template>
    <div>
        <h2>Carousel</h2>
        <div class="row">Page Id: {{pageId}}</div>
        <div class="row">Block Id: {{blockId}}</div>
        <div class="row">Applet Name: {{appletName}}</div>
        <div class="row">Data Attributes {{dataAttributes}}</div>
    </div>
</template>
