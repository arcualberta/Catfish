<script lang="ts">
    import { defineComponent, ref} from 'vue'
    import { useStore } from 'vuex'

    //import { Actions } from '../actions'
    //import { Mutations } from '../mutations'

    export default defineComponent({
        name: "FreeTextSearch",
        setup() {
            const store = useStore()
            const textValue = ref("");

            return {
                store,
                textValue,
                setText: (text: string) => store.commit('SET_FREE_TEXT', text),
                runSearch: () => store.dispatch('FRESH_SEARCH'),
            };
        },
    });
</script>

<template>
    <div class="input-group dir-text-search">
        <input type="text" class="form-control rounded" placeholder="searchText" aria-label="Search" aria-describedby="search-addon" v-model="textValue" @blur="runSearch()" />
        <!--<button type="button" class="btn btn-outline-primary" @click="executeSearch">search</button>-->

        <h3>Store</h3>
        {{JSON.stringify(store.state.rootState)}}

    </div>
</template>

