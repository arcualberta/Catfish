<template>
    <KeywordFilter :keywords="keywords"/>
</template>

<script lang="ts">
    import { defineComponent, ref } from 'vue';
    import KeywordFilter from './components/KeywordFilter.vue';

    export default defineComponent({
        name: "App",
        components: {
            KeywordFilter
        },
        actions: {

        },
        setup() {
            console.log('App setup')

            //Definiting reactive variables
            const pageId = ref('')
            const blockId = ref('')
            const keywords = ref([''])

            pageId.value = "a0de9368-add6-4677-a119-27f6cc942ad3";
            blockId.value = "8622971C-3B80-44BF-8B65-F911CFFBAA85";

            const loadKeywords = async () => {
                try {
                    let api = "https://localhost:44385/api/tilegrid/keywords/page/" + pageId.value + "/block/" + blockId.value;
                    const res = await fetch(api);
                    keywords.value = await res.json();
                }
                catch (err) {
                    console.log('Data loading error ', err)
                }
            }
            loadKeywords();

            return { keywords, pageId, blockId }
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
