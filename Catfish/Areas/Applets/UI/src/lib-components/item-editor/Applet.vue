<script lang="ts">
    import { defineComponent } from 'vue';
    import props from '../shared/props'

    export default defineComponent({
        name: "ItemEditor",
        components: {
        },
        props,
        setup(p, ctx) {
            console.log('Editor setup ...')
            console.log('props: ', p)
            console.log('context: ', ctx)
        },
        mounted() {
            console.log('Editor mounted ...')
        }
    });
</script>

<template>
    <div>
        <h2>Item Ediror</h2>
        <div class="row">Item Id: {{itemId}}</div>
    </div>
</template>
