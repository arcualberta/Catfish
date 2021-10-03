<template>
    <div class="data-filter">
        <ul>
            <li v-for="item in keywords" :key="item">
                <input type="checkbox" :value="item" v-model="selectedKeywords" @change="handleKeywordChange" /> 
                <label>{{ item }}</label>
            </li>
        </ul>
    </div>
</template>
<script lang="ts">
    import { defineComponent, ref, PropType } from "vue";
    import { Actions, SearchParams } from '../store/defs/actions';
    import { useStore } from '../store';

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
            const store = useStore()

            if (localStorage.selectedKeywords)
                selectedKeywords.value = localStorage.selectedKeywords.split('|');

            console.log('Keyword Panel setup')
            const handleKeywordChange = () => {
                console.log('keywords changed', selectedKeywords.value.length)
                //const searchParams: SearchParams = {
                //    keywords: selectedKeywords.value,
                //    offset: 0,
                //    max: 25
                //}
                console.log("Search Params: ", selectedKeywords.value)
                store.dispatch(Actions.FILTER_BY_KEYWORDS, selectedKeywords.value)
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
