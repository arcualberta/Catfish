<script lang="ts">
    import { defineComponent, ref } from 'vue'
    import { useStore } from 'vuex'

    import { state } from './store/state'
    import { Actions, actions } from './store/actions'
    import { mutations } from './store/mutations'
    import { getters } from './store/getters'
    import KeywordFilter from './components/KeywordFilter.vue'

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

            const s = useStore()
            s.dispatch(Actions.INIT_FILTER, { pageId: p.pageId, blockId: p.blockId });

            const keywordQueryModel = ref(s.state.keywordQueryModel);
            return { keywordQueryModel };
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
    <div>
        <KeywordFilter :query-model="keywordQueryModel"/>
        <ItemList />
    </div>
</template>

