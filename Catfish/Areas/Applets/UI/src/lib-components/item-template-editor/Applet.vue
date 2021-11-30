<script lang="ts">
    import { defineComponent } from 'vue'
    import { useStore } from 'vuex';

    import { state } from './store/state'
    import { actions } from './store/actions'
    import { getters } from './store/getters'
    import { mutations } from './store/mutations'
    import props, { QueryParameter } from '../shared/props'

    import ItemTemplate from './components/ItemTemplate.vue'


    export default defineComponent({
        name: "ItemTemplateEditor",
        components: {
            ItemTemplate
        },
        props,
        setup(p) {
            const store = useStore();

            console.log('Item Template Editor setup ...');
            console.log('props: ', JSON.stringify(p));
            const queryParams = p.queryParameters as QueryParameter;

            store.dispatch("SET_ID", queryParams.id);

            //load the data
            store.dispatch("LOAD_TEMPLATE");

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
    <h3>Item Template Editor</h3>
    <div>Item Template ID: {{queryParameters.id}}</div>
    <ItemTemplate />
</template>