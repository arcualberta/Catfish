<script lang="ts">
    import { defineComponent, ref } from 'vue'
    import { useStore } from 'vuex'

    import { store/*, useStore*/ } from './store'
    import { state } from './store/defs/state'
    import { Actions, actions } from './store/defs/actions'
    import { mutations } from './store/defs/mutations'
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
        mounted() {
            console.log('Keyword Search mounted ...')
            //this.pageId = this.$el.parentElement.getAttribute("page-id");
            //this.blockId = this.$el.parentElement.getAttribute("block-id");

            const s = useStore();
            s.dispatch(Actions.INIT_FILTER, { pageId: this.pageId, blockId: this.blockId });
        },
        storeConfig: {
            store,
            state,
            actions,
            mutations
        }
    });
</script>

<template>
    <div>
        <h2>Keyword Search</h2>
        <KeywordFilter :query-model="keywordQueryModel"/>
    </div>
</template>

