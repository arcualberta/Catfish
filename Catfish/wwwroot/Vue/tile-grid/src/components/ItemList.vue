<template>
    <div class="data-items">
        <h1>Item List</h1>
        <div class="container row">
            <div v-for="item in items" :key="item" class="col-md-4 col-sm-3">
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
            return { items: computed(() => store.state.items), }
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
