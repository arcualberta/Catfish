<script lang="ts">
    import { defineComponent, computed, PropType } from "vue";

    import props from '../../../shared/props'

    import { useStore } from 'vuex';
    import { Mutations } from '../../store/mutations';
    import { ePage } from '../../store/state';
    import { KeywordIndex } from "../../models/keywords";

    import KeywordPanel from "./KeywordPanel.vue"

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
            KeywordPanel
        },
        setup(p) {
            const store = useStore();

            let hexColorList = p.colorScheme ? p.colorScheme?.split(',').map(function (c) {
                return c.trim();
            }) : null;

          

            return {
                filterByKeyword: (cIndex: number, fIndex: number, vIndex: number) => {
                    store.commit(Mutations.CLEAR_KEYWORD_SELECTIONS);
                    store.commit(Mutations.SELECT_KEYWORD, { containerIndex: cIndex, fieldIndex: fIndex, valueIndex: vIndex } as KeywordIndex);
                    store.commit(Mutations.SET_ACTIVE_PAGE, ePage.List)
                },
                keywordQueryModel: computed(() => store.state.keywordQueryModel),
                hexColorList
            }
        },
    });
</script>

<template>
   <h2>Home View</h2>
   
    <KeywordPanel :hexColorList="hexColorList" />
</template>

