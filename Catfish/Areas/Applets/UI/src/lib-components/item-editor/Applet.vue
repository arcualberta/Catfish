<script lang="ts">
    import { defineComponent } from 'vue';
    import props, { QueryParameter } from '../shared/props'

    export default defineComponent({
        name: "ItemEditor",
        components: {
        },
        props,
        setup(p, ctx) {
            console.log('Editor setup ...')
            console.log('props: ', p)
            console.log('context: ', ctx)

            const queryParameters = p.queryParameters as QueryParameter;

            return {
                queryParameters
            }
        },
        mounted() {
            console.log('Editor mounted ...')
      }
    });
</script>

<template>
    <div>
        <h2>Item Ediror</h2>
        <div>Item ID: {{queryParameters.id}}</div>
    </div>
</template>
