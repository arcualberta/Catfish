<script lang="ts">
    import { defineComponent, computed, ref } from 'vue'
    import { useStore } from 'vuex';

    import { state, UserPermission } from './store/state'
    import { actions, Actions } from './store/actions'
    import { mutations, Mutations } from './store/mutations'
    import { getters } from './store/getters'
    import props, { QueryParameter, DataAttribute } from '../shared/props'

    import FieldContainerViewer from '../shared/components/display/FieldContainer.vue'
    import FieldContainerEditor from '../shared/components/editor/FieldContainer.vue'

    import { FieldContainer as FieldContainerModel } from '../shared/models/fieldContainer'


    export default defineComponent({
        name: "ItemDetails",
        components: {
            FieldContainerViewer,
            FieldContainerEditor
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

            const editMode = ref(false);

            const isEditable = (fc: FieldContainerModel): boolean => {

                if (editMode.value) {
                    const permissionsOfFieldContainer = (store.state.permissionList as UserPermission[]).find(p => p.formId == fc.id)?.permissions;
                    return permissionsOfFieldContainer?.find(p => p.action == "Update") != null;
                }
                else
                    return false;               
            }

            return {
                store,
                queryParams,
                dataItem: computed(() => store.state.item),
                getContainerName,
                isAdmin,
                editMode,
                isEditable
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
    <div class="controls">
        <button @click="editMode = !editMode" class="btn btn-primary"><span v-if="editMode">View</span><span v-else>Edit</span></button>
    </div>
    <div v-for="ms in dataItem?.metadataSets?.$values">
        <h4>{{getContainerName(ms)}}</h4>
        <FieldContainerEditor v-if="isEditable(ms)" :model="ms" />
        <FieldContainerViewer v-else :model="ms" />
    </div>
    <div v-for="di in dataItem?.dataContainer?.$values">
        <h4>{{getContainerName(di)}}</h4>
        <FieldContainerEditor v-if="isEditable(di)" :model="di" />
        <FieldContainerViewer v-else :model="di" />
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
    .controls{
        text-align:right;
    }
</style>

