<template>
    <div class="data-filter">
        <h1>Keyword Filter</h1>
        <ul>
            <li v-for="k in keywords" :key="k">
                <input type="checkbox" value="k" /> 
                <label>{{k}}</label>
            </li>
        </ul>
    </div>
</template>
<script lang="ts">
    import { defineComponent } from "vue";
    export default defineComponent({
        name: "KeywordFilter",
        setup() {
            const keywords = ['key 1', 'key 2', 'key 3']
            return { keywords }
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
