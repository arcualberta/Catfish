<script lang="ts">
	import { defineComponent, computed/*, ref*/ } from "vue";

    import { useStore } from 'vuex';
    import { Actions } from '../store/actions'

    export default defineComponent({
		name: "IndexingPanel",

        props: {},
        setup() {
            const store = useStore()

            return {
				reindexData: () => store.dispatch(Actions.REINDEX_DATA),
                reindexPages: () => store.dispatch(Actions.REINDEX_PAGES),
                indexStatus: computed(() => store.state.indexingStatus)
			}
        },
    });
</script>

<template>
    <h5 class="card-title">Data Indexing</h5>
    <p class="card-text">With supporting text below as a natural lead-in to additional content.</p>
    <button v-if="indexStatus.dataIndexingInprogress" href="#" class="btn btn-danger" disabled>Indexing In-progress</button>
    <a v-else href="#" class="btn btn-primary" @click="reindexData">Reindex Data</a>
    <br />
    <br />
    <h5 class="card-title">Page Indexing</h5>
    <p class="card-text">With supporting text below as a natural lead-in to additional content.</p>
    <button v-if="indexStatus.pageIndexingInprogress" href="#" class="btn btn-danger" disabled>Indexing In-progress</button>
    <a v-else href="#" class="btn btn-primary" @click="reindexPages">Reindex Pages</a>
</template>


