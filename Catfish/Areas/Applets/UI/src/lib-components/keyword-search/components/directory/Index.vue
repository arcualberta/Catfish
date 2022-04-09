<script lang="ts">
    import { defineComponent, PropType, computed } from "vue";
    import { useStore } from 'vuex';

    import props from '../../../shared/props';
    import { State, ePage } from '../../store/state';
    import { Mutations } from '../../store/mutations';

    import HomeView from './HomeView.vue';
    import ListView from './ListView.vue';
    import DetailsView from './DetailsView.vue';


    export default defineComponent({
        name: "DirectoryView",
        components: {
            HomeView,
            ListView,
            DetailsView
        },
        props: {
            colorScheme: {
                type: null as PropType<string> | null,
                required: false
            },
            ...props
        },
        setup(p) {
            const store = useStore();
            const queryParameters = p.queryParameters;
            console.log(JSON.stringify(queryParameters))
            if (queryParameters?.page) {
                const page = queryParameters?.page as unknown as ePage;
                store.commit(Mutations.SET_ACTIVE_PAGE, page);
            }
            return {
                ePage,
                visit: (page: ePage) => store.commit(Mutations.SET_ACTIVE_PAGE, page), 
                page: computed(() => (store.state as State).activePage),
            }
        }
    });
</script>

<template>
    <nav >
        <a href="#" @click="visit(ePage.Home)">Home</a> | 
        <a href="#" @click="visit(ePage.List)">Explore</a>
    </nav>
    <HomeView v-if="page == ePage.Home" :data-attributes="dataAttributes" :query-parameters="queryParameters" />
    <ListView v-if="page == ePage.List" />
    <DetailsView v-if="page == ePage.Details" />

    <!--<ListView v-if="page == ePage.List" :data-attributes="dataAttributes" :query-parameters ="queryParameters"/>
    <DetailsView v-if="page == ePage.Details" :data-attributes="dataAttributes" :query-parameters ="queryParameters"/>-->
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