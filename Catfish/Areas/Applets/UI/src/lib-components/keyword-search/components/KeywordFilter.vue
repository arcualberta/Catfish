<script lang="ts">
    import { defineComponent, ref, PropType, computed, /* toRefs,*/ watch } from "vue";
    import { useStore } from 'vuex';
    import { Actions } from '../store/actions';
    //import { SearchParams } from "../models"
    import { KeywordQueryModel } from '../models/keywords'

    //import { Guid } from "guid-typescript";
   // import ItemList from './ItemList.vue';

    export default defineComponent({
        name: "KeywordFilter",
        components: {
          //ItemList
        },
        props: {
            queryModel: null as null | PropType<KeywordQueryModel>
        },
        setup(props) {

            console.log("KeywordFilter props: ", props)

            const store = useStore();
            console.log("Store: ", store)

            const runFreshSearch = () => {
                store.dispatch(Actions.FILTER_BY_KEYWORDS);
            }

            //const nextPage = () => {
            //    store.dispatch(Actions.NEXT_PAGE);
            //}

            //const previousPage = () => {
            //    store.dispatch(Actions.PREVIOUS_PAGE);
            //}

            const queryModel = ref(store.state.keywordQueryModel);
            watch(queryModel, () => {
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


            return {
                //searchParams,
                //previousPage,
                //nextPage,
                //dispatchSearch,
                runFreshSearch,
                keywordQueryModel: computed(() => store.state.keywordQueryModel),
            //    items: computed(() => store.state.searchResult?.items),
            //    count: computed(() => store.state.searchResult?.count),
            //    first: computed(() => store.state.searchResult?.first),
            //    last: computed(() => store.state.searchResult?.last)
            }
        }
    });
</script>

<template>
    <div v-for="(container, cIdx) in keywordQueryModel?.containers" :key="container">
        <div v-if="keywordQueryModel?.containers.length > 1 && container?.name?.length > 0">{{container.name}}</div>
        <div v-for="(field, fIdx) in container.fields" :key="field" class="mb-3">
            <div v-if="field.name.length > 0" class="font-weight-bold">{{field.name}}</div>
            <div v-for="(value, vIdx) in field.values" :key="value">
                <input type="checkbox" :value="value" v-model="keywordQueryModel.containers[cIdx].fields[fIdx].selected[vIdx]" @change="runFreshSearch" />
                <label class="ml-1">{{ value }}</label>
            </div>
        </div>
    </div>
</template>