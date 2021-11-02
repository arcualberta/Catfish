<template>
    <KeywordSearch :page-id="pageId" :block-id="blockId" :applet-name="appletName" :data-attributes="dataAttributes"></KeywordSearch>
    <Carousel :page-id="pageId" :block-id="blockId" :applet-name="appletName" :data-attributes="dataAttributes"></Carousel>
</template>

<script lang="ts">
    import { defineComponent, ref } from 'vue';
    import { Guid } from 'guid-typescript';
    import { useStore } from 'vuex';
    import { Mutations } from './store/defs/mutations';
    import { DataAttribute } from './models';

    import Carousel from './applets/carousel/App.vue';
    import KeywordSearch from './applets/keyword-search/App.vue'

    export default defineComponent({
        name: 'App',
        components: {
            Carousel,
            KeywordSearch
        },
        setup() {

            //Definiting reactive variables
            const appletName = ref('')
            const pageId = ref(Guid.EMPTY)
            const blockId = ref(Guid.EMPTY)
            const dataAttributes = ref([] as DataAttribute[])

            return { appletName, pageId, blockId, dataAttributes }
        },
        mounted() {
            this.pageId = this.$el.parentElement.getAttribute("page-id");
            this.blockId = this.$el.parentElement.getAttribute("block-id");
            this.appletName = this.$el.parentElement.getAttribute("data-applet-name");

            const dataAttributeNames = Array.from(this.$el.parentElement.attributes)
                .filter(att => (att as Attr).name.startsWith("data-"));
            dataAttributeNames.forEach(att => {
                const attrib = (att as Attr);
                this.dataAttributes.push({
                    name: attrib.name.substring(5),
                    value: attrib.value
                });
            });
            console.log("Parent Attributes: ", this.dataAttributes);

            const store = useStore();
            store.commit(Mutations.INIT_APPLET, { pageId: this.pageId, blockId: this.blockId, appletName: this.appletName });
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
