<script lang="ts">
    import { defineComponent, onMounted, ref } from 'vue'
    import { useStore } from 'vuex'

    import { state } from './store/state'
    import { Actions, actions } from './store/actions'
    import { Mutations, mutations } from './store/mutations'
    import { getters } from './store/getters'
    import { SearchParams } from "./models"
    import DictionaryView from './components/DictionaryView.vue'
    import ListView from './components/ListView.vue'
    import FreeTextSearch from './components/FreeTextSearch.vue'
    import DirectoryView from './components/directory/Index.vue'


    import props, { DataAttribute, QueryParameter } from '../shared/props'


    export default defineComponent({
        name: "Applet",
        components: {
            DirectoryView,
            DictionaryView,
            ListView,
            FreeTextSearch
        },
        props,
        setup(p) {
            console.log('Keyword Search setup ...', p)

            const dataAttributes = p.dataAttributes as DataAttribute;

            const displayFormat = dataAttributes["display-format"] as string;
            const blogTitle = dataAttributes["block-title"] as string;
            const blogDescription = dataAttributes["block-description"] as string;
            const enableFreeTextSearch = dataAttributes["enable-freetext-search"] as string;
            const hexColors = dataAttributes["hex-color-list"] as string;


            //We need to use store in this setup method. so let's load it first.
            const store = useStore()

            //Storing the page and block IDs in the store
            store.commit(Mutations.SET_SOURCE, { pageId: p.pageId, blockId: p.blockId });

            //See if we can load a SearchParams object from local storage
            
            const searchParamsStr = localStorage.getItem(store.getters.searchParamStorageKey);
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
                dataAttributes,
                queryParameters: p.queryParameters as QueryParameter,
                keywordQueryModel,
                displayFormat,
                blogTitle,
                blogDescription,
                enableFreeTextSearch,
                hexColors
              
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
    <DirectoryView v-if="displayFormat === 'Directory'" data-attributes="dataAttributes" query-parameters="queryParameters" />

    <div v-if="displayFormat === 'Dictionary'">
        <h1 class="dir-title">{{blogTitle}}</h1>
        <div class="dir-description">{{blogDescription}}</div>
        <div v-if="enableFreeTextSearch === true">
            <FreeTextSearch />
        </div>
        <DictionaryView :colorScheme="hexColors" />

    </div>
    <div class="row" v-if="displayFormat === 'List'">
        <ListView />
    </div>

</template>

