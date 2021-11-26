<script lang="ts">
    import { defineComponent } from 'vue'

    import { state } from './store/state'
    import { actions } from './store/actions'
    import { getters } from './store/getters'
    import { mutations } from './store/mutations'
    import props, { QueryParameter } from '../shared/props'


    export default defineComponent({
        name: "ItemTemplateEditor",
        components: {

        },
        props,
        setup(p) {
            console.log('Item Template Editor setup ...');
            console.log('props: ', JSON.stringify(p));
            let queryParams = p.queryParameters as QueryParameter;
            let pid = queryParams['pid'];
            console.log(" pid : " + pid)
           
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
</template>