<script lang="ts">
	import { defineComponent, computed/*, ref*/ } from "vue";

    import { useStore } from 'vuex';
    import { Actions } from '../store/actions'
    import { eIndexingStatus } from '../models'
    import { State } from '../store/state'

    export default defineComponent({
		name: "IndexingPanel",

        props: {},
        setup() {
            const store = useStore();
             
            return {
                reindexData: () => store.dispatch(Actions.REINDEX_DATA),
                reindexPages: () => store.dispatch(Actions.REINDEX_PAGES),
                isPageIndexingReady: computed(() => (store.state as State).indexingStatus.pageIndexingStatus == eIndexingStatus.Ready),
                isDataIndexingReady: computed(() => (store.state as State).indexingStatus.dataIndexingStatus == eIndexingStatus.Ready)
            }
        },
    });
</script>

<template>
    <h5 class="card-title">Data Indexing</h5>
    <p class="card-text">With supporting text below as a natural lead-in to additional content.</p>
    <button v-if="isDataIndexingReady" class="btn btn-primary" @click="reindexData">Reindex</button>
    <button v-else class="btn btn-danger" disabled>Indexing In-progress</button>
    <br />
    <br />
    <h5 class="card-title">Page Indexing</h5>
    <p class="card-text">With supporting text below as a natural lead-in to additional content.</p>
    <button v-if="isPageIndexingReady" class="btn btn-primary" @click="reindexPages">Reindex</button>
    <button v-else class="btn btn-danger" disabled>Indexing In-progress</button>
</template>


