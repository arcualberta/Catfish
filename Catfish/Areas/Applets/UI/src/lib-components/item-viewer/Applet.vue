<script lang="ts">
    import { defineComponent } from 'vue'
    import { useStore } from 'vuex';

    import { state } from './store/state'
    import { actions, Actions } from './store/actions'
    import { getters } from './store/getters'
    import { mutations, Mutations } from './store/mutations'
    import props, { QueryParameter } from '../shared/props'

    //import Item from './components/Item.vue'


    export default defineComponent({
        name: "ItemViewer",
        components: {
            //Item
        },
        props,
        setup(p) {
            const store = useStore();

            console.log('Item Viewer setup ...');
            console.log('props: ', JSON.stringify(p));
            const queryParams = p.queryParameters as QueryParameter;

            store.commit(Mutations.SET_ID, queryParams.id);

            //load the data
            store.dispatch(Actions.LOAD_ITEM);

            return {
                queryParams
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
    <h3>Item Viewer Editor</h3>
    <div>Item ID: {{queryParameters.iid}}</div>
    <!--<ItemTemplate />-->
</template>