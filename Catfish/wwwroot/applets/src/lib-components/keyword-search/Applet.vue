<script lang="ts">
    import { defineComponent } from 'vue'
    import { useStore } from 'vuex'

    import { state } from './store/defs/state'
    import { Actions, actions } from './store/defs/actions'
    import { mutations } from './store/defs/mutations'

    import props from '../shared/props'

    export default defineComponent({
        name: "Applet",
        components: {
        },
        props,
        setup(p) {
            console.log('Keyword Search setup ...', p)

            const s = useStore()
            console.log('Store ...', s)
            s?.dispatch(Actions.INIT_FILTER, { pageId: p.pageId, blockId: p.blockId });



        },
        mounted() {
            console.log('Keyword Search mounted ...')
        },
        storeConfig: {
            state: state,
            actions: actions,
            mutations: mutations
        }
    });
</script>

<template>
    <div>
        <h2>Keyword Search</h2>
        <div class="row">Page Id: {{pageId}}</div>
        <div class="row">Block Id: {{blockId}}</div>
        <div class="row">Data Attributes {{dataAttributes}}</div>
    </div>
</template>

