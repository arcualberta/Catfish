<script lang="ts">
    import { defineComponent, computed, PropType, onMounted } from "vue";
    import { Guid } from 'guid-typescript'

    import props from '../../../shared/props'

    import { useStore } from 'vuex';
    import { Actions } from '../../store/actions';
    import { Mutations } from '../../store/mutations';
    import { Mutations as ItemViewerMutations } from '../../../item-viewer/store/mutations';
    import { KeywordIndex, Keyword } from "../../models/keywords";
    import { State, ePage } from '../../store/state';

    export default defineComponent({
        name: "ListView",
        props: {
            colorScheme: {
                type: null as PropType<string> | null,
                required: false
            },
            ...props
        },
        setup(p) {
            const store = useStore();

            let hexColorList = p.colorScheme ? p.colorScheme?.split(',').map(function (c) {
                return c.trim();
            }) : null;

            onMounted(() => {
                const btns = Array.from(document.getElementsByClassName(`dir-keyword-button`));
                let i = 0;
                btns.forEach((b) => {
                    if (hexColorList !== null && hexColorList[i] !== "") {
                        b.setAttribute("style", "background-color: " + hexColorList[i]);
                        i++
                        i = i <= hexColorList.length - 1 ? i : 0;

                    } else {

                        let color = "hsla(" + ~~(360 * Math.random()) + "," + "70%," + "80%,1)";
                        b.setAttribute("style", "background-color: " + color);
                    }
                });

                store.dispatch(Actions.FRESH_SEARCH);
            })

            return {
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
                viewDetails: (itemId: Guid) => {
                    store.commit(ItemViewerMutations.SET_ID, itemId);
                    store.commit(Mutations.SET_ACTIVE_PAGE, ePage.Details);
                },
                keywordQueryModel: computed(() => store.state.keywordQueryModel),
                items: computed(() => store.state.searchResult?.items),
                results: computed(() => store.state.searchResult),
                selectedKeywords: computed(() => {
                    const ret = [] as Keyword[];
                    (store.state as State).keywordQueryModel?.containers.forEach((cont, cIdx) =>
                        cont.fields.forEach((field, fIdx) =>
                            field.values.forEach((val, vIdx) => {
                                if (store.getters.isKeywordSelected(cIdx, fIdx, vIdx))
                                    ret.push({ index: { containerIndex: cIdx, fieldIndex: fIdx, valueIndex: vIdx }, value: val } as Keyword);
                            })
                        )
                    )
                    return ret;
                })
            }
        }
    });
</script>

<template>
    <h2>List View</h2>
    <div class="row">
        <div class="col-md-6 row">
            <div class="col-md-4">
                <b>Selected Keywords</b>
                <div v-for="keyword in selectedKeywords" :key="keyword.index.containerIndex + '_' + keyword.index.valueIndex">
                    {{keyword.value}}
                    <i class="fa fa-remove" @click="removeKeyword(keyword.index)"></i>
                </div>
            </div>

            <div class="col-md-8">
                <b>Entries</b>
                <div v-for="item in items" :key="item.id">
                    <a href="#" @click="viewDetails(item.id)">{{item.id}}</a>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div v-for="(container, cIdx) in keywordQueryModel?.containers" :key="container.id">
                <div v-for="(field, fIdx) in container.fields" :key="field.id" class="row keywordContainer">
                    <span v-for="(value, vIdx) in field.values" :key="value.id" class="dir-keyword">
                        <button @click="addKeyword(cIdx, fIdx, vIdx)" class="dir-keyword-button" ref="dirBtn">{{ value }}</button>
                    </span>
                </div>
            </div>
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
    .keywordContainer::-webkit-scrollbar {
        width: 12px;
        height: 5px;
        overflow-x: scroll;
        background-color: transparent;
    }

    .keywordContainer::-webkit-scrollbar-track {
        background-color: transparent;
        -webkit-box-shadow: inset 0 0 6px rgba(0,0,0,0.05);
    }

    .keywordContainer::-webkit-scrollbar-thumb {
        background-color: grey;
        border-radius: 10px;
        /* border: 1px solid Green;*/
    }

    .keywordContainer::-webkit-scrollbar-track-piece:end {
        margin-right: 75px;
    }

    .keywordContainer::-webkit-scrollbar-track-piece:start {
        margin-left: 175px;
    }
</style>