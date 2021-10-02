<template>
    <KeywordFilter :keywords="keywords" />
    <ItemList />
</template>

<script lang="ts">
    import { defineComponent, ref } from 'vue';
    import KeywordFilter from './components/KeywordFilter.vue';
    import ItemList from './components/ItemList.vue';
    import { useStore } from './store';
    import { Actions } from './store/defs/actions';

    export default defineComponent({
        name: "App",
        components: {
            KeywordFilter,
            ItemList
        },
        setup() {
            //console.log('App setup')

            //Definiting reactive variables
            const keywords = ref([''])

            const loadKeywords = async (pageId: string, blockId: string) => {
                try {
                    let api = window.location.origin + `/api/tilegrid/keywords/page/${pageId}/block/${blockId}`;
                    console.log('Loading keywords: ', api)

                    const res = await fetch(api);
                    keywords.value = await res.json();
                }
                catch (err) {
                    console.log('Data loading error ', err)
                }
            }

            return { keywords, loadKeywords }
        },
        mounted() {
            //console.log('App mounted')
            let pageId = this.$el.parentElement.getAttribute("page-id");
            let blockId = this.$el.parentElement.getAttribute("block-id");
            this.loadKeywords(pageId, blockId);

            console.log("localStorage.selectedKeywords: ", localStorage.selectedKeywords)

            const store = useStore()
            store.dispatch(Actions.FILTER_BY_KEYWORDS, localStorage.selectedKeywords)
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
