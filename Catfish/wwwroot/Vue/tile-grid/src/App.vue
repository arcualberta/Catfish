<template>
    <KeywordFilter pageId="a0de9368-add6-4677-a119-27f6cc942ad3" blockId="" />
</template>

<script lang="ts">
    import { defineComponent } from "vue";
    import KeywordFilter from "./components/KeywordFilter.vue";

    export default defineComponent({
        name: "App",
        components: {
            KeywordFilter
        },
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
