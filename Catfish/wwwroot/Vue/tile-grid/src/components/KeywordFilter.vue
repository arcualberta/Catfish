<template>
    <div class="data-filter">
        <h1>Keyword Filter</h1>
        <ul>
            <li v-for="item in keywords" :key="item">
                <input type="checkbox" :value="item" v-model="selectedKeywords" @change="handleKeywordChange" /> 
                <label>{{ item }}</label>
            </li>
        </ul>
        <div>{{selectedKeywords}}</div>
    </div>
</template>
<script lang="ts">
    import { defineComponent, ref, PropType } from "vue";
    import { Actions } from '../store/defs/actions'
    import { useStore } from '../store'

    export default defineComponent({
        name: "KeywordFilter",
        props: {
            keywords: {
                required: true,
                type: Array as PropType<string[]>
            }
        },
        setup() {

            const selectedKeywords = ref([]);
            const store = ref(useStore())

            console.log('Keyword Panel setup')
            const handleKeywordChange = (e: MouseEvent) => {
                console.log('keywords changed', selectedKeywords.value.length)
                store.value.dispatch(Actions.FILTER_BY_KEYWORDS, selectedKeywords.value)
            }

            return { handleKeywordChange, selectedKeywords }
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
