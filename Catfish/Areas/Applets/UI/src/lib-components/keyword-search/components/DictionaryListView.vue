<script lang="ts">
    import { defineComponent, computed, onMounted, PropType } from "vue";
    import dayjs from "dayjs";

    import { useStore } from 'vuex';
    import { Actions } from '../store/actions'
    import { KeywordQueryModel } from '../models/keywords'

    export default defineComponent({
        name: "DictionaryListView",

        props: {
            model: {
                type: null as PropType<KeywordQueryModel> | null,
                required: true
            },
            hexColorList: {
                type: null as PropType<string[]> | null,
                required: false
            },
        },
        setup(p) {
            const store = useStore();

            const runFreshSearch = () => store.dispatch(Actions.FRESH_SEARCH);
            let items= store.state.searchResult?.items;

            console.log(JSON.stringify(items));
            //const nextPage = () => store.dispatch(Actions.NEXT_PAGE);
            //const previousPage = () => store.dispatch(Actions.PREVIOUS_PAGE);
            //const freshSearch = (pageSize: number) => store.dispatch(Actions.FRESH_SEARCH, pageSize);

           /* const selectedPageSize = ref(25);*/
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

            })
            return {
                runFreshSearch,
               // items: computed(() => store.state.searchResult?.items),
                keywordQueryModel: computed(() => p.model)
                //freshSearch,
                //nextPage,
                //previousPage,
                //selectedPageSize,
                //count: computed(() => store.state.searchResult?.count),
                //first: computed(() => store.state.searchResult?.first),
                //last: computed(() => store.state.searchResult?.last)
            }
        },

        methods: {
            formatDate(dateString: string) {
                const date = dayjs(dateString);
                return date.format('MMM DD, YYYY');
            },
            addKeyword(cIdx: Number | any, fIdx: Number | any, vIdx: Number | any) {
                this.keywordQueryModel.containers[cIdx].fields[fIdx].selected[vIdx] = !this.keywordQueryModel.containers[cIdx].fields[fIdx].selected[vIdx];

                this.runFreshSearch;
            }
        }
    });
</script>

<template>
    <div class="dictionaryListView ">
        <h3>Ditionary List View</h3>
        <div class="col-12 row ">
            <div class="col-2 lefNav">
                <div v-for="(container, cIdx) in keywordQueryModel?.containers" :key="container">

                    <div v-for="(field, fIdx) in container.fields" :key="field" class="row keywordContainer">

                        <div v-for="(value, vIdx) in field.values" :key="vIdx" >
                            <div v-if="field.selected[vIdx] == true" ><span class=" selectedKeyword ">{{ value }}</span><span class="fa fa-times" @click="addKeyword(cIdx, fIdx, vIdx)" ></span></div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-5 contentList grey-BG">
                {{model}}
                <!--<div v-for="item in items" :key="item.id">
                    <div class="item">
                        <h3 class="item-title">
                            <a v-if='item.detailedViewUrl?.length > 0' v-bind:href="item.detailedViewUrl">{{item.title}}</a>
                            <span v-else>{{item.title}}</span>
                        </h3>
                        <div class="item-date">{{formatDate(item.date)}}</div>
                        <h5 class="item-subtitle">{{item.subtitle}}</h5>
                        <div class="categories">
                            <span v-for="cat in item.categories" class="badge rounded-pill bg-dark text-white m-1">
                                {{cat}}
                            </span>
                        </div>
                        <div class="content">{{item.content}}</div>
                    </div>
                </div>-->
            </div>
            <div class="col-md-4 searchNav">
                <div v-for="(container, cIdx) in keywordQueryModel?.containers" :key="container">

                    <div v-for="(field, fIdx) in container.fields" :key="field" class="row keywordContainer">

                        <span v-for="(value, vIdx) in field.values" :key="value" class="dir-keyword">
                            <button @click="addKeyword(cIdx, fIdx, vIdx)" class="dir-keyword-button" ref="dirBtn">{{ value }}</button>
                        </span>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>


<style scoped>

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
    
    .fa-times{
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
        overflow-x: scroll;
        overflow-y: visible;
        white-space: nowrap;
        position: relative;
        display: inline-block;
        height: 100px;
        width: 400px;
        align-content: center;
        vertical-align:baseline;
    }

    .dir-keyword {
        display: inline-block;
        margin-top: 15px;
    }

    .dir-keyword-button {
        position: relative;
        color: black;
        text-align: center;
        border-radius: 60px;
        padding: 5px;
        white-space: normal;
        font-size: small;
        width:60px;
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

