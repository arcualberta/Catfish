<script lang="ts">
    import { defineComponent, ref, PropType, computed, /* toRefs,*/ watch } from "vue";
    import { useStore } from 'vuex';
    import { Actions } from '../store/actions';
    import { KeywordQueryModel } from '../models/keywords'

    export default defineComponent({
        name: "KeywordFilter",
        props: {
            queryModel: null as null | PropType<KeywordQueryModel>
        },
        setup(props) {

            console.log("KeywordFilter props: ", props)

            const store = useStore();
            console.log("Store: ", store)

            const runFreshSearch = () => {
                store.dispatch(Actions.FILTER_BY_KEYWORDS);
            }

            const queryModel = ref(store.state.keywordQueryModel);
            watch(queryModel, () => {
                if (queryModel) {
                    console.log("KeywordFilter updated queryModel: ", queryModel)
                }
            })

            return {
                runFreshSearch,
                keywordQueryModel: computed(() => store.state.keywordQueryModel)
            }
        }
    });
</script>

<template>
    <div v-for="(container, cIdx) in keywordQueryModel?.containers" :key="container">
        <div v-if="keywordQueryModel?.containers.length > 1 && container?.name?.length > 0">{{container.name}}</div>
        <div v-for="(field, fIdx) in container.fields" :key="field" class="mb-3">
            <div v-if="field.name.length > 0" class="font-weight-bold">{{field.name}}</div>
            <div v-for="(value, vIdx) in field.values" :key="value">
                <input type="checkbox" :value="value" v-model="keywordQueryModel.containers[cIdx].fields[fIdx].selected[vIdx]" @change="runFreshSearch" />
                <label class="ml-1">{{ value }}</label>
            </div>
        </div>
    </div>
</template>