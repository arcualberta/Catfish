<template>
    <div class="data-item">
        <h3>{{item.title}}</h3>
        This is a tile.
    </div>
</template>
<script lang="ts">
    import { defineComponent, PropType } from "vue";
    import { Item } from "../store/defs/state"

    export default defineComponent({
        name: "Tile",
        props: {
            item: null as null | PropType<Item>
        },
    });
</script>
<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
/*    h3 {
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
*/</style>
