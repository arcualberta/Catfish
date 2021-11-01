<template>
    <div class="row">
        <h2>Keyword Search</h2>
        <div>Page Id: {{pageIdx}}</div>
        <!--<KeywordFilter :keywords="keywords" :page-id="pageId" :block-id="blockId" />-->
    </div>
</template>

<script lang="ts">
    import { defineComponent, ref, PropType, computed, toRefs, watch } from 'vue';
    import { Guid } from 'guid-typescript';
    //import KeywordFilter from './components/KeywordFilter.vue';
    //import { useStore } from './store';
    //import { Actions } from './store/defs/actions';

    export default defineComponent({
        name: "App",
        components: {
        //    KeywordFilter
        },
        props: {
            pageId: {
                required: true,
                type: null as PropType<Guid> | null
            },
            blockId: {
                required: true,
                type: null as PropType<Guid> | null
            },
            appletName: {
                required: true,
                type: String
            }
        },
        setup() {

        //    //Definiting reactive variables
        //    const keywords = ref([''])
        //    const pageId = ref(Guid.EMPTY)
        //    const blockId = ref(Guid.EMPTY)

        //    return { keywords, pageId, blockId }
        //},
        //mounted() {
        //    this.pageId = this.$el.parentElement.getAttribute("page-id");
        //    this.blockId = this.$el.parentElement.getAttribute("block-id");

        //    const store = useStore();
        //    store.dispatch(Actions.INIT_FILTER, { pageId: this.pageId, blockId: this.blockId });
        }
    });
</script>

<style lang="scss">
    #app {
        font-family: Avenir, Helvetica, Arial, sans-serif;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        text-align: center;
        color: #2c3e50;
        margin-top: 60px;
    }
</style>
