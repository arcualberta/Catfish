<script lang="ts">
    import { defineComponent, ref, computed} from 'vue'
    import { useStore } from 'vuex'

    import { Actions } from '../actions'
    import { Mutations } from '../mutations'

    export default defineComponent({
        name: "FreeTextSearch",
        setup() {
            const store = useStore()
            const textValue = ref("");

            return {
                store,
                state: computed(() => store.state),
                textValue,
                setText: (text: string) => store.commit(Mutations.SET_FREE_TEXT, text),
                runSearch: () => {
                    store.commit(Mutations.SET_FREE_TEXT, textValue.value)
                    store.dispatch(Actions.FRESH_SEARCH)
                },
            };
        },
    });
</script>

<template>
    <div class="input-group dir-text-search">
        <input type="text" class="form-control rounded" placeholder="searchText" aria-label="Search" aria-describedby="search-addon" v-model="textValue" @blur="runSearch()" />
        <!--<button type="button" class="btn btn-outline-primary" @click="executeSearch">search</button>-->

        <h3>Search Module State</h3>
        {{JSON.stringify(state)}}

    </div>
</template>

