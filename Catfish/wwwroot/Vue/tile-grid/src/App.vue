<template>
    <KeywordFilter :keywords="keywords" :page-id="pageId" :block-id="blockId" />
    <ItemList />
</template>

<script lang="ts">
    import { defineComponent, ref } from 'vue';
    import KeywordFilter from './components/KeywordFilter.vue';
    import ItemList from './components/ItemList.vue';
import { Guid } from 'guid-typescript';

    export default defineComponent({
        name: "App",
        components: {
            KeywordFilter,
            ItemList
        },
        setup() {

            //Definiting reactive variables
            const keywords = ref([''])
            const pageId = ref(Guid.EMPTY)
            const blockId = ref(Guid.EMPTY)

            const loadKeywords = async () => {
                try {
                    let api = window.location.origin + `/api/tilegrid/keywords/page/${pageId.value}/block/${blockId.value}`;
                    console.log('Loading keywords: ', api)

                    const res = await fetch(api);
                    keywords.value = await res.json();
                }
                catch (err) {
                    console.log('Data loading error ', err)
                }
            }

            return { keywords, pageId, blockId, loadKeywords }
        },
        mounted() {
            this.pageId = this.$el.parentElement.getAttribute("page-id");
            this.blockId = this.$el.parentElement.getAttribute("block-id");
            this.loadKeywords();

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
