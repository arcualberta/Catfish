<script lang="ts">
    import { defineComponent, computed } from 'vue'
    import { useStore } from 'vuex';

    import { state } from './store/state'
    import { actions, Actions } from './store/actions'
    import { mutations, Mutations } from './store/mutations'
    import props, { QueryParameter, DataAttribute } from '../shared/props'

    import FieldContainer from '../shared/components/display/FieldContainer.vue'


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
            store.dispatch(Actions.LOAD_ITEM);

            return {
                store,
                queryParams,
                dataItem: computed(() => store.state.item),
                isAdmin
            }
        },
        storeConfig: {
            state,
            actions,
            mutations

        },
        methods: {
        }
    });
</script>

<template>
    
    <div>

        <div v-for="value in dataItem.template.metadataSets.$values">
            <div v-for="metadataName in value.name.values.$values">
                <h4>Metadata Set : {{metadataName.value}}</h4>
                <div v-for="fieldValue in value.fields">
                    <!--<FieldContainer :model="fieldValue" />-->
                </div>
            </div>


        </div>
        <!--{{dataItem.dataContainer}}-->
        <div v-for="value in dataItem.dataContainer.$values">
            <div v-for="dataContainer in value.name.values.$values">
                <h4> Data Container : {{dataContainer.value}}</h4>
            </div>
    <!--<div v-for="fieldValue in value.fields">
    <FieldContainer :model="fieldValue" />
    </div>-->
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

