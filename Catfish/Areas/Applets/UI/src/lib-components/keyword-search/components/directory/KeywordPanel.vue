<script lang="ts">
   // import { Guid } from 'guid-typescript'
    import { defineComponent, onMounted, computed, PropType} from "vue";
    import { useStore } from 'vuex';
    import { Mutations } from '../../store/mutations';
    import { ePage } from '../../store/state';
    import { KeywordIndex } from "../../models/keywords";
    import props from '../../../shared/props'
    import { Actions } from '../../store/actions';
   // import props, { QueryParameter } from '../../../shared/props'

   // import { Mutations as ItemViewerMutations } from '../../../item-viewer/store/mutations';
   // import { Actions as ItemViewerActions } from '../../../item-viewer/store/actions';
   // import { State } from '../../store/state';

    export default defineComponent({
        name: "KeywordPanel",
        props: {
            hexColorList: {
                type: null as PropType<string[]> | null,
                required: false
            },
            runAction: {
                type: null as PropType<string> | null,
                required: false
            },
            className: {
                type: null as PropType<string> | null,
                required: false
            },
            ...props
        },
        setup(p) {
            const store = useStore();
            //let hexColorList = p.colorScheme ? p.colorScheme?.split(',').map(function (c) {
            //    return c.trim();
            //}) : null;

            //TODO: Update this view template to represent the keyword panel in a way that we can
            // embed it the Home, List, and Details views.

            //const queryParameters = p.queryParameters as QueryParameter;
            //if (queryParameters && (queryParameters["iid"] as string)?.length > 0)
            //    store.commit(ItemViewerMutations.SET_ID, queryParameters["iid"] as unknown as Guid);

            //const itemId = computed(() => (store.state as State).id);

            onMounted(() => {
                const btns = Array.from(document.getElementsByClassName(`dir-keyword-button`));
                let length = p.hexColorList ? p.hexColorList.length : 0;
                let i = 0;
                btns.forEach((b) => {
                    if (p.hexColorList !== null) {
                        let color = p.hexColorList ? p.hexColorList[i] : "";
                        b.setAttribute("style", "background-color: " + color);
                        i++
                        i = i <= length - 1 ? i : 0;

                    } else {

                        let color = "hsla(" + ~~(360 * Math.random()) + "," + "70%," + "80%,1)";
                        b.setAttribute("style", "background-color: " + color);
                    }

                });

              //  const className = p.className;
                //console.log("className: " + className);
             

            })

            return {
                filterByKeyword: (cIndex: number, fIndex: number, vIndex: number) => {
                    store.commit(Mutations.CLEAR_KEYWORD_SELECTIONS);
                    store.commit(Mutations.SELECT_KEYWORD, { containerIndex: cIndex, fieldIndex: fIndex, valueIndex: vIndex } as KeywordIndex);
                    store.commit(Mutations.SET_ACTIVE_PAGE, ePage.List)
                },
                keywordQueryModel: computed(() => store.state.keywordQueryModel),
                addKeyword: (cIndex: number, fIndex: number, vIndex: number) => {
                    if (!store.getters.isKeywordSelected(cIndex, fIndex, vIndex)) {
                        store.commit(Mutations.SELECT_KEYWORD, { containerIndex: cIndex, fieldIndex: fIndex, valueIndex: vIndex } as KeywordIndex);
                        store.dispatch(Actions.FRESH_SEARCH);
                    }
                },
                removeKeyword: (index: KeywordIndex) => {
                    store.commit(Mutations.CLEAR_KEYWORD, index);
                    store.dispatch(Actions.FRESH_SEARCH);
                },
                className: p.className
            }
        },

        //methods: {
        //    addKeyword: (cIndex: number, fIndex: number, vIndex: number) => {
        //        if (!this.store.getters.isKeywordSelected(cIndex, fIndex, vIndex)) {
        //            this.store.commit(Mutations.SELECT_KEYWORD, { containerIndex: cIndex, fieldIndex: fIndex, valueIndex: vIndex } as KeywordIndex);
        //            this.store.dispatch(Actions.FRESH_SEARCH);
        //        }
        //    },
        //    removeKeyword: (index: KeywordIndex) => {
        //        this.store.commit(Mutations.CLEAR_KEYWORD, index);
        //        this.store.dispatch(Actions.FRESH_SEARCH);
        //    },
        //}
    });
</script>

<template>
    <div v-for="(container, cIdx) in keywordQueryModel?.containers" :key="container">
        <div v-for="(field, fIdx) in container.fields" :key="field"   :class="className? 'row ' + className : 'row keywordContainer'">
            <span v-for="(value, vIdx) in field.values" :key="value" class="dir-keyword">
                <button v-if="runAction === 'filterByKeyword'" @click="filterByKeyword(cIdx, fIdx, vIdx)" class="dir-keyword-button" ref="dirBtn">{{ value }}</button>
                <button v-else @click="addKeyword(cIdx, fIdx, vIdx)" class="dir-keyword-button" ref="dirBtn">{{ value }}</button>
            </span>
        </div>
    </div>
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
        scroll-behavior: smooth;
        align-content: center;
    }

    .dir-keyword {
        display: inline-block;
        margin-top: 15px;
        margin-right: 5px;
    }

    .dir-keyword-button {
        position: relative;
        color: Black;
        font-size: 0.80em;
        text-align: center;
        border-radius: 60px;
        padding-top: 30px;
        padding-bottom: 30px;
        padding-left: 10px;
        padding-right: 10px;
        max-width: 150px;
        white-space: normal;
    }

        .dir-keyword-button:focus {
            background-color: yellow;
        }

        .dir-keyword-button:hover {
            transform: scale(1.2);
            z-index: 100;
            opacity: 90%;
            text-decoration: underline;
        }


    /* Works on Chrome, Edge, and Safari */
    .keywordContainer::-webkit-scrollbar, .keywordContainerSmall::-webkit-scrollbar {
        width: 12px;
        height: 5px;
        overflow-x: scroll;
        background-color: transparent;
    }

    .keywordContainer::-webkit-scrollbar-track, .keywordContainerSmall::-webkit-scrollbar-track {
        background-color: transparent;
        -webkit-box-shadow: inset 0 0 6px rgba(0,0,0,0.05);
    }

    .keywordContainer::-webkit-scrollbar-thumb, .keywordContainerSmall::-webkit-scrollbar-thumb {
        background-color: grey;
        border-radius: 10px;
        /* border: 1px solid Green;*/
    }

    .keywordContainer::-webkit-scrollbar-track-piece:end, .keywordContainerSmall::-webkit-scrollbar-track-piece:end {
        margin-right: 75px;
    }

    .keywordContainer::-webkit-scrollbar-track-piece:start, .keywordContainerSmall::-webkit-scrollbar-track-piece:start {
        margin-left: 175px;
    }

    /* smaller version*/
    .keywordContainerSmall {
        overflow-x: scroll;
        overflow-y: visible;
        white-space: nowrap;
        position: relative;
        display: inline-block;
        height: 90px;
        width: 100%;
        scroll-behavior: smooth;
        align-content: center;
    }
    .keywordContainerSmall > .dir-keyword {
            display: inline-block;
            margin-top: 15px;
            margin-right: 5px;
            font-size: medium;
        }

   .keywordContainerSmall > .dir-keyword > .dir-keyword-button {
                position: relative;
                color: Black;
                font-size: 0.80em;
                text-align: center;
                border-radius: 60px;
                padding-top: 15px;
                padding-bottom: 15px;
                padding-left: 10px;
                padding-right: 10px;
                max-width: 150px;
                white-space: normal;
            }
</style>