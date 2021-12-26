<script lang="ts">
	import { defineComponent, onMounted/*, ref*/ } from 'vue'
    import { useStore } from 'vuex'

    import { state } from './store/state'
    import { Actions, actions } from './store/actions'
    import { mutations } from './store/mutations'
    import { getters } from './store/getters'

    import props from '../shared/props'

	import { default as IndexingPanel } from './components/IndexingPanel.vue'

    export default defineComponent({
        name: "Applet",
        components: {
			IndexingPanel
        },
        props,
        setup(p) {
            console.log('Process Manager setup ...', p)
            const store = useStore();

            onMounted(() => store.dispatch(Actions.FETCH_REINDEX_STATUS));

        },
        storeConfig: {
            state,
            actions,
            mutations,
            getters
        }
    });
</script>

<template>
    <div class="card">
        <div class="card-header">
            Indexing Processes
        </div>
        <div class="card-body">
            <IndexingPanel />
        </div>
    </div>
</template>

