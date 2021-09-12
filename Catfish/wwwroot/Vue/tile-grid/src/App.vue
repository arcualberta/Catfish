<template>
    <KeywordFilter />
    <p>{{pageId}}</p>
    <p>{{blockId}}</p>
</template>

<script lang="ts">
    import { defineComponent, ref } from "vue";
    import KeywordFilter from "./components/KeywordFilter.vue";

    export default defineComponent({
        name: "App",
        components: {
            KeywordFilter
        },
        setup(props, context) {
            console.log('Setup')

            //Definiting reactive variables
            const pageId = ref(null)
            const blockId = ref(null)


            //// Attributes (Non-reactive object)
            //console.log(context.attrs)

            //// Slots (Non-reactive object)
            //console.log(context.slots)

            //// Emit Events (Method)
            //console.log(context.emit)

            //console.log('Setup end')

            return { pageId, blockId }
        },
        mounted() {
            console.log('App mounted')
            let topChild = this.$refs.apptopchild as Element;
            console.log(topChild);
            let appRoot = topChild.parentElement;
            console.log(appRoot);
            console.log("App Root")
            console.log(this.$root?.$attrs)
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
