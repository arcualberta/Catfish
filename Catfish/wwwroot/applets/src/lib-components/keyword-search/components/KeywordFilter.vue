<script lang="ts">
    import { defineComponent, ref, PropType,/* computed, toRefs,*/ watch } from "vue";
    import { useStore } from 'vuex';
    //import { Actions } from '../store/defs/actions';
    //import { SearchParams } from "../models"
    import { KeywordQueryModel } from '../models/keywords'

    //import { Guid } from "guid-typescript";
    //import ItemList from './ItemList.vue';

    export default defineComponent({
        name: "KeywordFilter",
        components: {
        //    ItemList
        },
        props: {
            queryModel: null as null | PropType<KeywordQueryModel>
        },
        setup(props) {

            const store = useStore();
            console.log("Store: ", store)
            //const queryModel = ref(store.state.keywordQueryModel);
            const queryModel = ref(store.state.queryModel);

            console.log("KeywordFilter props: ", props)
            console.log("KeywordFilter initial queryModel: ", queryModel)

            watch(queryModel, () => {
                console.log("Watch called queryModel: ", queryModel)

                if (queryModel) {
                    console.log("KeywordFilter updated queryModel: ", queryModel)
                }
            })

            //const searchParams = ref({} as SearchParams);
            //const { pageId } = toRefs(props);
            //const { blockId } = toRefs(props);

            ////If the Local Storage contains search-params object, load it. Otherwise, create a default one.
            //console.log("localStorage.keywordSearchParams: ", localStorage.keywordSearchParams)
            //searchParams.value = (localStorage.keywordSearchParams)
            //    ? JSON.parse(localStorage.keywordSearchParams)
            //    : { keywords: [], offset: 0, max: 25 };

            //watch([pageId, blockId], () => {
            //    if (pageId.toString() !== Guid.EMPTY && blockId.toString() !== Guid.EMPTY) {
            //        dispatchSearch()
            //    }
            //})

            //const store = useStore()

            //const runFreshSearch = () => {
            //    //When the keywords are changed, always set the search offset to zero.
            //    searchParams.value.offset = 0;
            //    dispatchSearch()
            //}

            //const previousPage = () => {
            //    searchParams.value.offset = Math.max(0, searchParams.value.offset - searchParams.value.max);
            //    dispatchSearch();
            //}

            //const nextPage = () => {
            //    //NOTE: The prepended + sign is needed in the following statement to enforce 
            //    //numerical addition instead of string concatenation
            //    searchParams.value.offset = +searchParams.value.offset + +searchParams.value.max;
            //    dispatchSearch();
            //}

            //const dispatchSearch = () => {
            //    //Save the search being carried out in the Local Storage.
            //    localStorage.keywordSearchParams = JSON.stringify(searchParams.value);

            //    //Overwtite any collection ID value saved in the local storage because if we rely on it,
            //    //we may use a wrong value from the cache if we ever change the collection 
            //    //in the piranha nlock configuration.
            //    searchParams.value.pageId = pageId.value;
            //    searchParams.value.blockId = blockId.value;

            //    store.dispatch(Actions.FILTER_BY_KEYWORDS, searchParams.value);
            //}

            //return {
            //    searchParams,
            //    runFreshSearch,
            //    previousPage,
            //    nextPage,
            //    dispatchSearch,
            //    keywordQueryModel: computed(() => store.state.keywordQueryModel),
            //    items: computed(() => store.state.searchResult?.items),
            //    count: computed(() => store.state.searchResult?.count),
            //    first: computed(() => store.state.searchResult?.first),
            //    last: computed(() => store.state.searchResult?.last)
            //}

            return { queryModel };
        }
    });
</script>

<template>
    <div>{{queryModel}}</div>
    <div class="col-md-3 text-left">
        <div v-for="(container, cIdx) in keywordQueryModel?.containers" :key="container">
            <div v-if="keywordQueryModel?.containers.length > 1 && container?.name?.length > 0">{{container.name}}</div>
            <div v-for="(field, fIdx) in container.fields" :key="field" class="mb-3">
                <div v-if="field.name.length > 0" class="font-weight-bold">{{field.name}}</div>
                <div v-for="(value, vIdx) in field.values" :key="value">
                    <input type="checkbox" :value="value" v-model="keywordQueryModel.containers[cIdx].fields[fIdx].selected[vIdx]" @change="runFreshSearch" />
                    <label class="ml-1">{{ value }}</label>
                </div>
            </div>
            <!--Container {{container}}-->
        </div>
    </div>
    <div class="col-md-9 mb-4">
        <div v-if="items?.length > 0">
            <span v-if="first > 1"><i class="fas fa-angle-double-left" @click="previousPage"></i></span>
            {{first}}-{{last}} of {{count}}
            <span v-if="count > last"><i class="fas fa-angle-double-right" @click="nextPage"></i></span>
            <span>
                <select v-model="searchParams.max" class="pull-right" @change="runFreshSearch">
                    <option>25</option>
                    <option>50</option>
                    <option>100</option>
                </select>
            </span>
        </div>
        <div v-else>No results found.</div>
        <ItemList />
    </div>

</template>