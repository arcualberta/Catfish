<script lang="ts">
    import { Guid } from 'guid-typescript'
    import { defineComponent, computed, onMounted } from "vue";
    import { useStore } from 'vuex';

    import props, { QueryParameter } from '../../../shared/props'

    import { Mutations as ItemViewerMutations } from '../../../item-viewer/store/mutations';
    import { Actions as ItemViewerActions } from '../../../item-viewer/store/actions';
    import { State } from '../../store/state';

    export default defineComponent({
        name: "DetailsView",
        props,
        setup(p) {
            const store = useStore();

            const queryParameters = p.queryParameters as QueryParameter;
            if (queryParameters && (queryParameters["iid"] as string)?.length > 0)
                store.commit(ItemViewerMutations.SET_ID, queryParameters["iid"] as unknown as Guid);

            const itemId = computed(() => (store.state as State).id);

            onMounted(() => {
                if (itemId)
                    store.dispatch(ItemViewerActions.LOAD_ITEM, itemId);
            })

            return {
                itemId,
                item: computed(() => (store.state as State).item),
            }
        },
    });
</script>

<template>
    <h2>Details View</h2>
    Item ID: {{JSON.stringify(itemId)}} <br /> <br />
    {{JSON.stringify(item)}}
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