<script lang="ts">
    import { defineComponent, ref } from 'vue'
    import { useStore } from 'vuex'

    import { state } from './store/defs/state'
    import { Actions, actions } from './store/defs/actions'
    import { mutations } from './store/defs/mutations'
    import { getters } from './store/defs/getters'
    import KeywordFilter from './components/KeywordFilter.vue'

    import props from '../shared/props'

    export default defineComponent({
        name: "Applet",
        components: {
            KeywordFilter
        },
        props,
        setup(p) {
            console.log('Keyword Search setup ...', p)

            const s = useStore()
            s.dispatch(Actions.INIT_FILTER, { pageId: p.pageId, blockId: p.blockId });

            const keywordQueryModel = ref(s.state.keywordQueryModel);
            return { keywordQueryModel };
        },
        //mounted() {
        //    console.log('Keyword Search mounted ...')

        //    const s = useStore();
        //    s.dispatch(Actions.INIT_FILTER, { pageId: this.pageId, blockId: this.blockId });
        //},
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
    </div>
</template>

