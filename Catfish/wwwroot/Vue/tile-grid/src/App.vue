<template>
    <KeywordFilter :keywords="keywords" :collection-id="collectionId" />
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
            const collectionId = ref(Guid.EMPTY)

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

            const setCollectionId = (id: string): void => {
                collectionId.value = id
            }

            return { keywords, collectionId, setCollectionId, loadKeywords }
        },
        mounted() {
            let pageId = this.$el.parentElement.getAttribute("page-id");
            let blockId = this.$el.parentElement.getAttribute("block-id");
            this.collectionId = this.$el.parentElement.getAttribute("collection-id");
           // this.setCollectionId(collectionId)
            this.loadKeywords(pageId, blockId);
            console.log("Vue App Mounted")

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
