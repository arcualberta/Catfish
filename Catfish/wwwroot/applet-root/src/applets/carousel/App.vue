<template>
    <div>
        <h2>Carousel</h2>
        <div class="row">Page Id: {{pageId}}</div>
        <div class="row">Block Id: {{blockId}}</div>
        <div class="row">Applet Name: {{appletName}}</div>
        <div class="row">Data Attributes {{dataAttributes}}</div>
    </div>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';
    import props from '../shared/props'

    export default defineComponent({
        name: "App",
        components: {
        },
        props,
        setup(props) {
            console.log('Carousel setup ...')
        },
        mounted() {
            console.log('Carousel mounted ...')
        }
    });
</script>

<style lang="scss">
    #app {
        font-family: Avenir, Helvetica, Arial, sans-serif;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        text-align: center;
        color: #2c3e50;
        margin-top: 60px;
    }
</style>
