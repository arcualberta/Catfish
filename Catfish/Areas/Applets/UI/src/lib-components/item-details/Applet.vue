<script lang="ts">
    import { defineComponent, computed } from 'vue'
    import { useStore } from 'vuex';

    import { state } from './store/state'
    import { actions, Actions } from './store/actions'
    import { mutations, Mutations } from './store/mutations'
    import { getters } from './store/getters'
    import props, { QueryParameter, DataAttribute } from '../shared/props'

    import FieldContainer from '../shared/components/display/FieldContainer.vue'
    import { FieldContainer as FieldContainerModel } from '../shared/models/fieldContainer'


    export default defineComponent({
        name: "ItemDetails",
        components: {
            FieldContainer
        },
        props,
        setup(p) {
            const store = useStore();
            const dataAttributes = p.dataAttributes as DataAttribute;
            console.log('Item Details setup ...');
            console.log('props: ', JSON.stringify(p));
            const isAdmin = dataAttributes["is-admin"] as string;
            console.log('isAdmin: ', isAdmin);
            const queryParams = p.queryParameters as QueryParameter;

            store.commit(Mutations.SET_ID, queryParams.iid);

            //load the data
            store.dispatch(Actions.GET_USER_ACTIONS);
            store.dispatch(Actions.LOAD_ITEM);

            const getContainerName = (fc: FieldContainerModel) => {
                return fc.name?.values.$values.map(txt => txt.value).join(" | ");
            }

            return {
                store,
                queryParams,
                dataItem: computed(() => store.state.item),
                getContainerName,
                isAdmin
            }
        },
        storeConfig: {
            state,
            actions,
            mutations,
            getters
        },
        methods: {
        }
    });
</script>

<template>
    
    <div>

        <div v-for="ms in dataItem?.metadataSets?.$values">
            <h4>{{getContainerName(ms)}}</h4>
            <FieldContainer :model="ms" />
        </div>
        <div v-for="di in dataItem.dataContainer.$values">
            <h4>{{getContainerName(di)}}</h4>
            <FieldContainer :model="di" />
        </div>
    </div>
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

