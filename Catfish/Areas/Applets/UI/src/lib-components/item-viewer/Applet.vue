<script lang="ts">
    import { defineComponent, computed } from 'vue'
    import { useStore } from 'vuex';

    import { state } from './store/state'
    import { actions, Actions } from './store/actions'
    import { getters } from './store/getters'
    import { mutations, Mutations } from './store/mutations'
    import props, { QueryParameter } from '../shared/props'

    import FieldContainer from '../shared/components/display/FieldContainer.vue'


    export default defineComponent({
        name: "ItemViewer",
        components: {
            FieldContainer
        },
        props,
        setup(p) {
            const store = useStore();

            console.log('Item Viewer setup ...');
            console.log('props: ', JSON.stringify(p));
            const queryParams = p.queryParameters as QueryParameter;

            store.commit(Mutations.SET_ID, queryParams.iid);

            //load the data
            store.dispatch(Actions.LOAD_ITEM);

            return {
                queryParams,
                dataItem: computed(() => store.getters.rootDataItem)
            }
        },
        storeConfig: {
            state,
            actions,
            mutations,
            getters
        }
    });
</script>

<template>
    <FieldContainer :model="dataItem" v-if="dataItem"/>
</template>

<style scoped>
    .field-name{
        font-weight:bold !important;
    }
</style>