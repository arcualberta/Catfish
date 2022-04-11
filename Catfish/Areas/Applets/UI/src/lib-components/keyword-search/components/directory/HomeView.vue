<script lang="ts">
    import { defineComponent, computed, PropType } from "vue";

    import  props,{ DataAttribute } from '../../../shared/props'

    import { useStore } from 'vuex';
    import { Mutations } from '../../store/mutations';
    import { ePage } from '../../store/state';
    import { KeywordIndex } from "../../models/keywords";

    import KeywordPanel from "./KeywordPanel.vue"
    import FreeTextSearch from '../FreeTextSearch.vue'

    export default defineComponent({
        name: "HomeView",
        props: {
            colorScheme: {
                type: null as PropType<string> | null,
                required: false
            },
            ...props
        },
        components: {
            KeywordPanel,
            FreeTextSearch
        },
        setup(p) {
            const store = useStore();
            const dataAttributes = p.dataAttributes as DataAttribute;
            const blogTitle = dataAttributes["block-title"] as string;
            const blogDescription = dataAttributes["block-description"] as string;
            const enableFreeTextSearch = dataAttributes["enable-freetext-search"] as string;
            const hexColors = dataAttributes["hex-color-list"] as string;

            let hexColorList = hexColors ? hexColors.split(',').map(function (c) {
                return c.trim();
            }) : null;

            console.log("home: " + JSON.stringify(enableFreeTextSearch));

            return {
                filterByKeyword: (cIndex: number, fIndex: number, vIndex: number) => {
                    store.commit(Mutations.CLEAR_KEYWORD_SELECTIONS);
                    store.commit(Mutations.SELECT_KEYWORD, { containerIndex: cIndex, fieldIndex: fIndex, valueIndex: vIndex } as KeywordIndex);
                    store.commit(Mutations.SET_ACTIVE_PAGE, ePage.List)
                },
                keywordQueryModel: computed(() => store.state.keywordQueryModel),
                hexColorList,
                blogTitle,
                blogDescription,
                enableFreeTextSearch
            }
        },
    });
</script>

<template>
    <h2>Home View</h2>
    <h1 class="dir-title">{{blogTitle}}</h1>
    <div class="dir-description">{{blogDescription}}</div>
    <div v-if="enableFreeTextSearch === true">
        <FreeTextSearch />
    </div>
    <KeywordPanel :hexColorList="hexColorList" runAction="filterByKeyword" />
</template>

