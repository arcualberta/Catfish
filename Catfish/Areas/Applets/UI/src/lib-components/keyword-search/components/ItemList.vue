<script lang="ts">
    import { defineComponent, computed, ref} from "vue";
    import { useStore } from 'vuex';

    import { Actions } from '../store/actions'

    export default defineComponent({
        name: "ItemList",
       
        props: {},
        setup() {
            const store = useStore()
           
            const nextPage = () => store.dispatch(Actions.NEXT_PAGE);
            const previousPage = () => store.dispatch(Actions.PREVIOUS_PAGE);
            const freshSearch = (pageSize: number) => store.dispatch(Actions.FRESH_SEARCH, pageSize);

            const selectedPageSize = ref(25);

            return {
                items: computed(() => store.state.searchResult?.items),
                freshSearch,
                nextPage,
                previousPage,
                selectedPageSize,
                count: computed(() => store.state.searchResult?.count),
                first: computed(() => store.state.searchResult?.first),
                last: computed(() => store.state.searchResult?.last)            }
        }
    });
</script>

<template>
    <div class="itemList">
        <div class="">
            <div v-if="items?.length > 0">
                <span v-if="first > 1"><i class="fas fa-angle-double-left" @click="previousPage"></i></span>
                {{first}}-{{last}} of {{count}}
                <span v-if="count > last"><i class="fas fa-angle-double-right" @click="nextPage"></i></span>
                <span>
                    <select v-model="selectedPageSize" class="pull-right" @change="freshSearch(Number(selectedPageSize))">
                        <option>25</option>
                        <option>50</option>
                        <option>100</option>
                    </select>
                </span>
            </div>
            <div v-else>No results found.</div>
        </div>
        <div v-for="item in items" :key="item.id">
            <div class="item">
                <h2>  {{item.title}}</h2>
                <h3>  {{item.subtitle}}</h3>
                <div>  {{item.content}}</div>
            </div>
        </div>
    </div>
</template>
