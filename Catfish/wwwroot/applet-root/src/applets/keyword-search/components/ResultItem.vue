<template>
    <div class="data-item">
        <a :href="item.detailedViewUrl"><img class="thumbnail" :src="item.thumbnail" /></a>
        <div class="cat-outer">
            <div class="cat-inner col-6">
                {{item.categories?.join(", ")}}
            </div>
        </div>
        <h3><a :href="item.detailedViewUrl">{{item.title}}</a></h3>
        <div class="content">
            {{ item.content }}
        </div>
    </div>
</template>
<script lang="ts">
    import { defineComponent, PropType } from "vue";
    //import { Item } from "../models"

    export default defineComponent({
        name: "Tile",
        props: {
            item: null as null | PropType<Item>
        }
    });
</script>
<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
    .data-item {
        box-shadow: 5px 5px 3px #E0E0E0;
        padding: 0;
        margin-bottom: 1.5em;
        text-align: left;
    }
    .data-item .thumbnail{
        width:100%;
        height:auto;
    }
    .data-item .cat-outer, .data-item .cat-inner {
        background-color: #112680;
        position: relative;
    }
    .data-item .cat-outer {
        height: 0.5em;
        top: -0.5em;
    }
    .data-item .cat-inner {
        height: 1.5em;
        top: -1.5em;
        position: relative;
        color: #ffffff;
        text-align: left;
        font-size: 0.8em;
        padding-top: 3px;
        padding-left: 5px;
        white-space: nowrap;
        width: 100%; /* IE6 needs any width */
        overflow: hidden; /* "overflow" value must be different from  visible"*/
        -o-text-overflow: ellipsis; /* Opera < 11*/
        text-overflow: ellipsis; /* IE, Safari (WebKit), Opera >= 11, FF > 6 */
    }
    .data-item h3, .data-item .content{
        margin-left: 5px;
    }
    .data-item h3 {
        font-size: 1.25em;
    }
    .data-item .content {
        display: -webkit-box;
        -webkit-line-clamp: 4;
        -webkit-box-orient: vertical;
        overflow: hidden;
    }

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
