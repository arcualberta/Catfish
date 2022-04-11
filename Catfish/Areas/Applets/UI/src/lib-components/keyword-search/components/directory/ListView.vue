<script lang="ts">
    import { defineComponent, computed, PropType, onMounted } from "vue";
    import { Guid } from 'guid-typescript'

    import props, { DataAttribute } from '../../../shared/props'

    import { useStore } from 'vuex';
    import { Actions } from '../../store/actions';
    import { Mutations } from '../../store/mutations';
    import { Mutations as ItemViewerMutations } from '../../../item-viewer/store/mutations';
    import { /*KeywordIndex,*/ Keyword } from "../../models/keywords";
    import { State, ePage } from '../../store/state';
    import KeywordPanel from "./KeywordPanel.vue"
    import FreeTextSearch from '../FreeTextSearch.vue'

    export default defineComponent({
        name: "ListView",
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
            //const blogTitle = dataAttributes["block-title"] as string;
            //  const blogDescription = dataAttributes["block-description"] as string;
            const enableFreeTextSearch = dataAttributes["enable-freetext-search"] as string;

            console.log("list view " + enableFreeTextSearch);

            const hexColors = dataAttributes["hex-color-list"] as string;

            let hexColorList = hexColors ? hexColors.split(',').map(function (c) {
                return c.trim();
            }) : null;
            onMounted(() => {

                store.dispatch(Actions.FRESH_SEARCH);
            })

            return {
                hexColorList,
                enableFreeTextSearch,
                //addKeyword: (cIndex: number, fIndex: number, vIndex: number) => {
                //    if (!store.getters.isKeywordSelected(cIndex, fIndex, vIndex)) {
                //        store.commit(Mutations.SELECT_KEYWORD, { containerIndex: cIndex, fieldIndex: fIndex, valueIndex: vIndex } as KeywordIndex);
                //        store.dispatch(Actions.FRESH_SEARCH);
                //    }
                //},
                //removeKeyword: (index: KeywordIndex) => {
                //    store.commit(Mutations.CLEAR_KEYWORD, index);
                //    store.dispatch(Actions.FRESH_SEARCH);
                //},
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
    <!--<h2>List View</h2>-->
    <div class="row">
        <div class="col-md-9 row">
            <div class="col-md-3">
                <!--<b>Selected Keywords</b>-->
                <div v-for="keyword in selectedKeywords" :key="keyword.index.containerIndex + '_' + keyword.index.valueIndex" >
                    <span class="selectedKeyword">{{keyword.value}}</span> 
                    <i class="fa fa-remove" @click="removeKeyword(keyword.index)"></i>
                </div>
            </div>

            <div class="col-md-8 grey-BG contentList">
                <b>Entries</b>
                <!--<div v-for="item in items" :key="item.id">
                    <a href="#" @click="viewDetails(item.id)">{{item.id}}</a>
                </div>-->
                <div v-for="item in items" :key="item.id">

                    <div class="item row">
                        <div>
                            <!--<img src="#" />-->
                            <i class="fas fa-image profileImg"></i>
                        </div>
                        <div class="profileInfo">
                            <h5 class="item-title">
                                <a href="#" @click="viewDetails(item.id)">{{item.subtitle}}</a>

                            </h5>
                           
                            <div class="content">{{item.content}}</div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
        <div class="col-md-3">
            <!--<div v-for="(container, cIdx) in keywordQueryModel?.containers" :key="container.id">
        <div v-for="(field, fIdx) in container.fields" :key="field.id" class="row keywordContainer">
            <span v-for="(value, vIdx) in field.values" :key="value.id" class="dir-keyword">
                <button @click="addKeyword(cIdx, fIdx, vIdx)" class="dir-keyword-button" ref="dirBtn">{{ value }}</button>
            </span>
        </div>
    </div>-->
            <div v-if="enableFreeTextSearch === true">
                <FreeTextSearch />
            </div>
            <KeywordPanel :hexColorList="hexColorList" />
        </div>
    </div>
</template>

<style scoped>
    .searchbar {
        background-color: #ececec;
        cursor: pointer;
        height: 40px;
        position: relative;
        width: 400px;
        display: inline-block;
        margin-top: 15px;
        padding: 12.5px
    }

    .profileInfo {
        margin-left: 10px;
        display: inline-block;
        margin-top: 22px;
        max-width:275px;
        font-size:large;
    }

    .profileImg {
        font-size: 1000%;
        margin-left: 5px;
    }

    .grey-BG {
        background-color: #ececec;
        border-radius: 30px;
        height: 500px;
        position: relative;
        width: 600px;
        width: 700px;
        margin-left: 20px;
        margin-top: 10px;
        margin-bottom: 20px;
        display: inline-block;
        overflow-y: scroll;
    }

    .fa-times {
        margin-left: 5px;
        font-size: medium;
    }

    .selectedKeyword {
        background-color: #ffdc0e;
        border-radius: 50px;
        position: relative;
        margin: 5px;
        padding: 5px;
        overflow-wrap: break-word;
        display: inline-block;
        font-size: medium;
    }

    .keywordContainer {
        
        height: 120px !important; 
        margin-left: 5px;
    }

   /* .dir-keyword {
        display: inline-block;
        margin-top: 15px;
        vertical-align: bottom;
    }

    .dir-keyword-button {
        position: relative;
        color: black;
        text-align: center;
        border-radius: 60px;
        padding: 5px;
        white-space: normal;
        font-size: small;
        width: 60px;
    }

        .dir-keyword-button:hover {
            transform: scale(1.2);
            z-index: 100;
            opacity: 90%;
            text-decoration: underline;
        }


    
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
    
    }

    .keywordContainer::-webkit-scrollbar-track-piece:end {
        margin-right: 75px;
    }

    .keywordContainer::-webkit-scrollbar-track-piece:start {
        margin-left: 175px;
    }
    */

    .contentList::-webkit-scrollbar {
        width: 7px;
        height: 5px;
        overflow-y: scroll;
        background-color: transparent;
    }

    .contentList::-webkit-scrollbar-track {
        background-color: transparent;
        -webkit-box-shadow: inset 0 0 6px rgba(0,0,0,0.02);
    }

    .contentList::-webkit-scrollbar-thumb {
        background-color: grey;
        border-radius: 10px;
        /* border: 1px solid Green;*/
    }

    .contentList::-webkit-scrollbar-track-piece:end {
        margin-bottom: 75px;
    }

    .contentList::-webkit-scrollbar-track-piece:start {
        margin-top: 175px;
    }
</style>
