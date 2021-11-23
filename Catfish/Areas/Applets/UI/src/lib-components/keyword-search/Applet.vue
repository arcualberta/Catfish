<script lang="ts">
    import { defineComponent, onMounted, ref } from 'vue'
    import { useStore } from 'vuex'

    import { state } from './store/state'
    import { Actions, actions } from './store/actions'
    import { mutations } from './store/mutations'
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

            const store = useStore()
            const searchParams = ref({} as SearchParams);


            store.dispatch(Actions.INIT_FILTER, { pageId: p.pageId, blockId: p.blockId });

            //if there's cache in the localStorage for selected keywords, use that to reset the state
            if (localStorage.getItem('keywordSearchParams')) {
                searchParams.value = JSON.parse(localStorage.keywordSearchParams);
                console.log("before calling save keyword action: " + searchParams.value)
                store.dispatch(Actions.SAVE_KEYWORDS, searchParams.value);
             } 

            const params = ref(searchParams.value); //const state = ref(store.state)
            const keywordQueryModel = ref(store.state.keywordQueryModel);


            onMounted(() => store.dispatch(Actions.FILTER_BY_KEYWORDS, searchParams.value));

            return {
                keywordQueryModel, params
            };
        },
        watch: {
            params(newVal) {
                //localStorage.setItem("keywordSearchParams", JSON.stringify(newVal));
                localStorage.keywordSearchParams = JSON.stringify(newVal);
            }
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

