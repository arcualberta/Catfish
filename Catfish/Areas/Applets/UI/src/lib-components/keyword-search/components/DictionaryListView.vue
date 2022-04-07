<script lang="ts">
    import { defineComponent, computed, ref, PropType } from "vue";
    import dayjs from "dayjs";

    import { useStore } from 'vuex';
    import { Actions } from '../store/actions'
    import { KeywordQueryModel } from '../models/keywords'

    export default defineComponent({
        name: "DictionaryListView",

        props: {
            model: {
                type: null as PropType<KeywordQueryModel> | null,
                required: false
            },
        },
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
                last: computed(() => store.state.searchResult?.last)
            }
        },

        methods: {
            formatDate(dateString: string) {
                const date = dayjs(dateString);
                return date.format('MMM DD, YYYY');
            }
        }
    });
</script>

<template>
    <div class="dictionaryList">
        <h3>Ditionary List View</h3>
        {{model}}
    </div>
</template>


