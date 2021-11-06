<script lang="ts">
    import { defineComponent } from 'vue';
    import props from '../shared/props'

    export default defineComponent({
        name: "Applet",
        components: {
        },
        props,
        setup() {
            console.log('Keyword Search setup ...')


        },
        mounted() {
            console.log('Keyword Search mounted ...')
        }
    });
</script>

<template>
    <div>
        <h2>Keyword Search</h2>
        <div class="row">Page Id: {{pageId}}</div>
        <div class="row">Block Id: {{blockId}}</div>
        <div class="row">Applet Name: {{appletName}}</div>
        <div class="row">Data Attributes {{dataAttributes}}</div>
    </div>
</template>

