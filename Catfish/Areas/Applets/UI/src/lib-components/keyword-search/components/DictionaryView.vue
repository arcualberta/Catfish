<script lang="ts">
    import { defineComponent, computed, /* watch*/ } from "vue";
    import { useStore } from 'vuex';
    import { Actions } from '../store/actions';

    export default defineComponent({
        name: "DictionaryView",
        setup() {

            const store = useStore();
            //console.log("Store: ", store)

            const runFreshSearch = () => store.dispatch(Actions.FRESH_SEARCH);

            return {
                runFreshSearch,
                keywordQueryModel: computed(() => store.state.keywordQueryModel),
                results: computed(() => store.state.searchResult)
            }
        },
        methods: {
            addKeyword(cIdx: Number | any, fIdx: Number|any, vIdx: Number|any) {
                this.keywordQueryModel.containers[cIdx].fields[fIdx].selected[vIdx] = !this.keywordQueryModel.containers[cIdx].fields[fIdx].selected[vIdx];
                this.runFreshSearch;
            }
        }
    });
</script>

<template>
    <h3>Dictionary View</h3>
    <div v-for="(container, cIdx) in keywordQueryModel?.containers" :key="container">
      
        <div v-for="(field, fIdx) in container.fields" :key="field" class="row keywordContainer">
            
            <span v-for="(value, vIdx) in field.values" :key="value" class="dir-keyword">
                <button @click="addKeyword(cIdx, fIdx, vIdx)" class="dir-keyword-button">{{ value }}</button>
            </span>
        </div>
    </div>

    {{keywordQueryModel}}

    <div>RESULTS</div>
    <div>{{results}}</div>

</template>

<style scoped>
    .keywordContainer {
        
        overflow-x: scroll;
        overflow-y: visible;
        white-space: nowrap;
        position: relative;
        display: inline-block;
        height: 150px;
        width: 100%;
    }
    .dir-keyword {
        display: inline-block;
        margin-top: 15px;
    }
    .dir-keyword-button {
        position: relative;
        color: grey;
        
        text-align: center;
        border-radius: 60px;
        padding-top: 30px;
        padding-bottom: 30px;
        padding-left: 10px;
        padding-right: 10px
    }

    .dir-keyword-button:focus {
        background-color: yellow;
    }
</style>