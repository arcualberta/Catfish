<template>
    <div class="data-items">
        <div v-if="items.length > 0" class="m-4">
            <span v-if="first > 0"><i class="fas fa-angle-double-left"></i></span>
            {{first}} to {{last}} of {{count}}
            <span v-if="count > last"><i class="fas fa-angle-double-right"></i></span>
        </div>
        <div class="container row">
            <div v-for="item in items" :key="item" class="col-md-3 col-sm-3">
                <tile :item="item"></tile>
            </div>
        </div>
    </div>
</template>
<script lang="ts">
    import { defineComponent, computed } from "vue";
    import { useStore } from '../store'
    import Tile from './Tile.vue'

    export default defineComponent({
        name: "ItemList",
        components: {
            Tile
        },
        setup() {
            const store = useStore()

            return {
                items: computed(() => store.state.searchResult?.items),
                count: computed(() => store.state.searchResult?.count),
                first: computed(() => store.state.searchResult?.first),
                last: computed(() => store.state.searchResult?.last)
            }
        }
    });
</script>
<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
    h3 {
        margin: 40px 0 0;
    }
    ul {
        list-style-type: none;
        padding: 0;
    }
    li {
        display: inline-block;
        margin: 0 10px;
    }
    a {
        color: #42b983;
    }
</style>
