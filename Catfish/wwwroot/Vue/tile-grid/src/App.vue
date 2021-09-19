<template>
    <KeywordFilter :keywords="keywords" />
</template>

<script lang="ts">
    import { useStore } from './store'
    import { Tile } from './store/defs/state'
    import { defineComponent, ref } from 'vue';
    import KeywordFilter from './components/KeywordFilter.vue';
    import { Guid } from 'guid-typescript';

    export default defineComponent({
        name: "App",
        components: {
            KeywordFilter
        },
        actions: {

        },
        setup() {
            //console.log('App setup')

            const store = ref(useStore())

            let t: Tile = {
                title: "",
                thumbnail: new URL("http://google.ca"),
                id: Guid.create(),
                content: "",
                created: new Date(),
                objectUrl: new URL("https://localhost:44385")

            }
            store.value.state.items.push(t);

            //Definiting reactive variables
            const keywords = ref([''])

            const loadKeywords = async (pageId: string, blockId: string) => {
                try {
                    let api = `https://localhost:44385/api/tilegrid/keywords/page/${pageId}/block/${blockId}`;
                    //console.log('Loading keywords: ', api)

                    const res = await fetch(api);
                    keywords.value = await res.json();
                }
                catch (err) {
                    //console.log('Data loading error ', err)
                }
            }

            return { store, keywords, loadKeywords }
        },
        mounted() {
            //console.log('App mounted')
            let pageId = this.$el.parentElement.getAttribute("page-id");
            let blockId = this.$el.parentElement.getAttribute("block-id");
            this.loadKeywords(pageId, blockId);
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
