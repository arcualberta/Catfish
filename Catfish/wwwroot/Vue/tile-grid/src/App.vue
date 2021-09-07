<template>
    <KeywordFilter :id="blockId"></KeywordFilter>
</template>

<script lang="ts">
    import { Options, Vue } from 'vue-class-component';
    import KeywordFilter from './components/KeywordFilter.vue';

    @Options({
        components: { KeywordFilter },
        props: { blockId: "" as string }
    })
    export default class App extends Vue {
        name: string = "Tile Grid";

        //////Initializing the app
        ////created() {

        ////    //Ref: https://next.vuex.vuejs.org/guide/actions.html#dispatching-actions
        ////    .$store.dispatch('loadFieldTemplates')
        ////    this.$store.dispatch('loadForm', this.id)
        ////}

    }
</script>

<style lang="scss">
#app {

}
</style>
