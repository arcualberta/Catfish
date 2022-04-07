<script lang="ts">
    import { defineComponent, computed, PropType, onMounted, ref } from "vue";
    import { useStore } from 'vuex';

    import props, { QueryParameter } from '../../../shared/props'
    import { Guid } from "guid-typescript";
    import { Actions } from '../../store/actions';

    enum ePage { Home = "", List = "List", Details = "Details" }

    export default defineComponent({
        name: "Directory",
        props: {
            colorScheme: {
                type: null as PropType<string> | null,
                required: false
            },
            ...props
        },
        setup(p) {

            const store = useStore();

            const queryParams = p.queryParameters as QueryParameter;

            const page = ref(queryParams["page"] as ePage);
            const entryId = ref(queryParams["entryId"] as Guid | null)
            const selectedKeywords = ref(queryParams["entryId"] as Guid | null)

            const runFreshSearch = () => store.dispatch(Actions.FRESH_SEARCH);
         
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

            })
            return {
                page,
                entryId,
                selectedKeywords,
                runFreshSearch,
                keywordQueryModel: computed(() => store.state.keywordQueryModel),
                results: computed(() => store.state.searchResult) 
            }
        },
        methods: {
            addKeyword(cIdx: Number | any, fIdx: Number | any, vIdx: Number | any) {
                this.keywordQueryModel.containers[cIdx].fields[fIdx].selected[vIdx] = !this.keywordQueryModel.containers[cIdx].fields[fIdx].selected[vIdx];
                this.runFreshSearch;
            },
            generateRandomColor() {
                return "hsla(" + ~~(360 * Math.random()) + "," + "70%," + "80%,1)";
            }
        }
    });
</script>

<template>

    <h1>Directory</h1>
 
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
        align-content:center;
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
       z-index:100;
       opacity:90%;
       text-decoration:underline;
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