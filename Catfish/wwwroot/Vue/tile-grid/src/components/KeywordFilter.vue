<template>
    <div class="data-filter">
        <ul>
            <li v-for="item in keywords" :key="item">
                <input type="checkbox" :value="item" v-model="searchParams.keywords" @change="runSearch" />
                <label>{{ item }}</label>
            </li>
        </ul>
        <div v-if="items?.length > 0" class="m-4">
            <span v-if="first > 1"><i class="fas fa-angle-double-left" @click="previousPage"></i></span>
            {{first}}-{{last}} of {{count}}
            <span v-if="count > last"><i class="fas fa-angle-double-right" @click="nextPage"></i></span>
            <span>
                <select v-model="searchParams.max" @change="runSearch" class="pull-right">
                    <option>25</option>
                    <option>50</option>
                    <option>100</option>
                </select>
            </span>
        </div>
    </div>
</template>
<script lang="ts">
    import { defineComponent, ref, PropType, computed } from "vue";
    import { Actions } from '../store/defs/actions';
    import { SearchParams } from "../models"
    import { useStore } from '../store';

    export default defineComponent({
        name: "KeywordFilter",
        props: {
            keywords: {
                required: true,
                type: Array as PropType<string[]>
            }
        },
        setup() {

            const searchParams = ref({} as SearchParams);

            //If the Local Storage contains search-params object, load it. Otherwise, create a default one.
            console.log("localStorage.keywordSearchParams: ", localStorage.keywordSearchParams)
            searchParams.value = (localStorage.keywordSearchParams)
                ? JSON.parse(localStorage.keywordSearchParams)
                : { keywords: [], offset: 0, max: 25 };

            const store = useStore()

            const runSearch = () => {
                //When the keywords are changed, always set the search offset to zero.
                searchParams.value.offset = 0;

                //Save the search being carried out in the Local Storage.
                localStorage.keywordSearchParams = JSON.stringify(searchParams.value);

                store.dispatch(Actions.FILTER_BY_KEYWORDS, searchParams.value);
            }

            const previousPage = () => {
                searchParams.value.offset = Math.max(0, searchParams.value.offset - searchParams.value.max);
                store.dispatch(Actions.FILTER_BY_KEYWORDS, searchParams.value);
            }

            const nextPage = () => {
                //NOTE: The prepended + sign is needed in the following statement to enforce 
                //numerical addition instead of string concatenation
                searchParams.value.offset = +searchParams.value.offset + +searchParams.value.max;
                store.dispatch(Actions.FILTER_BY_KEYWORDS, searchParams.value);
            }

            //During the initial load of the filter, load items based on settings in the local storage or default
            store.dispatch(Actions.FILTER_BY_KEYWORDS, searchParams.value);

            return {
                searchParams,
                runSearch,
                previousPage,
                nextPage,
                items: computed(() => store.state.searchResult?.items),
                count: computed(() => store.state.searchResult?.count),
                first: computed(() => store.state.searchResult?.first),
                last: computed(() => store.state.searchResult?.last)
            }
        }
    });
</script>
<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
    h3 {
        margin: 40px 0 0;
    }
    ul {
        list-style-type: none;
        padding: 0;
    }
    li {
        display: inline-block;
        margin: 0 10px;
    }
    a {
        color: #42b983;
    }
</style>
