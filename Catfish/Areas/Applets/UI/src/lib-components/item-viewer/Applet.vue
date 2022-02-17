<script lang="ts">
    import { Guid } from 'guid-typescript'
    import { defineComponent, computed } from 'vue'
    import { useStore } from 'vuex';

    import { state } from './store/state'
    import { actions, Actions } from './store/actions'
    import { getters } from './store/getters'
    import { mutations, Mutations } from './store/mutations'
    import props, { QueryParameter, DataAttribute } from '../shared/props'

    import FieldContainer from '../shared/components/display/FieldContainer.vue'


    export default defineComponent({
        name: "ItemViewer",
        components: {
            FieldContainer
        },
        props,
        setup(p) {
            const store = useStore();
            const dataAttributes = p.dataAttributes as DataAttribute;
            console.log('Item Viewer setup ...');
            console.log('props: ', JSON.stringify(p));
            const isAdmin = dataAttributes["is-admin"] as string;
            console.log('isAdmin: ', isAdmin);
            const queryParams = p.queryParameters as QueryParameter;

            store.commit(Mutations.SET_ID, queryParams.iid);

            //load the data
            store.dispatch(Actions.LOAD_ITEM);

            return {
                store,
                queryParams,
                dataItem: computed(() => store.getters.rootDataItem),
                isAdmin
            }
        },
        storeConfig: {
            state,
            actions,
            mutations,
            getters,

        },
        methods: {

            changeItemState(itemId: Guid) {

                if (confirm("Do you really want to delete this item?")) {
                    // this.store.dispatch(Actions.DELETE_CHILD_INSTANCE, itemToRemove);
                    console.log("id: " + itemId);
                    this.store.dispatch(Actions.CHANGE_STATE, itemId);
                }
            }
        }
    });
</script>

<template>
    <div class="text-right" v-if="isAdmin"><span class="fas fa-remove" @click="changeItemState(queryParams.iid);"></span></div>
    <FieldContainer :model="dataItem" v-if="dataItem" class="item"/>
</template>

<style scoped>
    .field-name {
        font-weight: bold !important;
    }
    .fa-remove {
        color: red;
        margin-left: 30px;
    }
</style>

