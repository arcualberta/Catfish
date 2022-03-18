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
                keywordQueryModel: computed(() => store.state.keywordQueryModel)
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
        <!-- <div v-if="keywordQueryModel?.containers.length > 1 && container?.name?.length > 0">{{container.name}}</div>-->
        <div v-for="(field, fIdx) in container.fields" :key="field" class="row keywordContainer">
            <!-- <div v-if="field.name.length > 0" class="font-weight-bold">{{field.name}}</div>-->
            <span v-for="(value, vIdx) in field.values" :key="value">
                <!--<input type="checkbox" :value="value" v-model="keywordQueryModel.containers[cIdx].fields[fIdx].selected[vIdx]" @change="runFreshSearch" />
                <label class="ml-1">{{ value }}</label>-->
                <button @click="addKeyword(cIdx, fIdx, vIdx)" class="dir-keyword">{{ value }}</button>
            </span>
        </div>
    </div>

    {{keywordQueryModel}}
</template>

<style scoped>
    .keywordContainer {
        width: 100%;
        overflow-x: auto;
        white-space: nowrap;
    }
</style>