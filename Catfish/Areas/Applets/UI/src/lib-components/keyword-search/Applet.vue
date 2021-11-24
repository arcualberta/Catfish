<script lang="ts">
    import { defineComponent, onMounted, ref } from 'vue'
    import { useStore } from 'vuex'

    import { state } from './store/state'
    import { Actions, actions } from './store/actions'
    import { Mutations, mutations } from './store/mutations'
    import { getters } from './store/getters'
    import KeywordFilter from './components/KeywordFilter.vue'
    import { SearchParams } from "./models"

    import ItemList from './components/ItemList.vue'

    import props from '../shared/props'

    export default defineComponent({
        name: "Applet",
        components: {
            KeywordFilter,
            ItemList
        },
        props,
        setup(p) {
            console.log('Keyword Search setup ...', p)

            //We need to use store in this setup method. so let's load it first.
            const store = useStore()

            //Storing the page and block IDs in the store
            store.commit(Mutations.SET_SOURCE, { pageId: p.pageId, blockId: p.blockId });

            //See if we can load a SearchParams object from local storage
            const searchParamsStr = localStorage.getItem(p.blockId?.toString() + "SearchParams");
            let searchParams;
            if (searchParamsStr && searchParamsStr.length > 0
                && (searchParams = JSON.parse(searchParamsStr) as SearchParams)
                && searchParams.keywords) {

                //Restoring the store state from data reloaded from the state
                store.commit(Mutations.SET_KEYWORDS, searchParams.keywords);
                store.commit(Mutations.SET_OFFSET, searchParams.offset);
                store.commit(Mutations.SET_PAGE_SIZE, searchParams.max);
            }
            else {
                //Dispatch an action to loaf keywords
                store.dispatch(Actions.INIT_FILTER);
            }

            //When the component is mounted, execute a search query based on the current patameters in the store.state.
            onMounted(() => store.dispatch(Actions.FILTER_BY_KEYWORDS));

            const keywordQueryModel = ref(store.state.keywordQueryModel);
            return {
                keywordQueryModel
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
    <div class="row">
        <div class="col-md-4 text-left">
            <KeywordFilter />
        </div>
        <div class="col-md-8">
            <ItemList />
        </div>
    </div>
</template>

