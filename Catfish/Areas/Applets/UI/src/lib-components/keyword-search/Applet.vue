<script lang="ts">
    import { defineComponent, ref, computed, onMounted} from 'vue'
    import { useStore } from 'vuex'

    import { state } from './store/state'
    import { Actions, actions } from './store/actions'
    import { mutations } from './store/mutations'
    import { getters } from './store/getters'
    import KeywordFilter from './components/KeywordFilter.vue'

    import ItemList from './components/ItemList.vue'

    import props from '../shared/props'

    export default defineComponent({
        name: "Applet",
        components: {
            KeywordFilter,
            ItemList
        },
        props,
        setup(propsVals) {
            console.log('Keyword Search setup ...', propsVals)

            const store = useStore()
            store.dispatch(Actions.INIT_FILTER, { pageId: propsVals.pageId, blockId: propsVals.blockId });

            const keywordQueryModel = ref(store.state.keywordQueryModel);
            const pageSize = ref(25);

            //Method that runs a fresh search that starts with index 0 and the already selected page size.
            const runFreshSearch = () => {
                console.log("called runFreshSearch");
                store.dispatch(Actions.FILTER_BY_KEYWORDS);
            }

            onMounted(() => {
                runFreshSearch()
            });

            return {
                runFreshSearch,
                keywordQueryModel,
                pageSize,
                count: computed(() => store.state.searchResult?.count),
                first: computed(() => store.state.searchResult?.first),
                last: computed(() => store.state.searchResult?.last)
            };
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
    <div>
        <div class="row">
            <div class="col-md-4">
                <KeywordFilter :query-model="keywordQueryModel" />
            </div>
            <div class="col-md-8">
                <div v-if="count > 0">
                    <span v-if="first > 1"><i class="fas fa-angle-double-left" @click="previousPage"></i></span>
                    {{first + 1}}-{{last + 1}} of {{count}}
                    <span v-if="count > last"><i class="fas fa-angle-double-right" @click="nextPage"></i></span>
                    <span>
                        <select v-model="pageSize" class="pull-right" @change="runFreshSearch">
                            <option>25</option>
                            <option>50</option>
                            <option>100</option>
                        </select>
                    </span>
                </div>
                <div v-else>No results found.</div>

                <ItemList />
            </div>
        </div>
    </div>
</template>

