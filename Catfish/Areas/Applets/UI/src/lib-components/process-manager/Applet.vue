<script lang="ts">
    import { defineComponent/*, onMounted, ref*/ } from 'vue'
    //import { useStore } from 'vuex'

    import { state } from './store/state'
    import { actions } from './store/actions'
    import { mutations } from './store/mutations'
    import { getters } from './store/getters'

    import props from '../shared/props'

    export default defineComponent({
        name: "Applet",
        components: {
        },
        props,
        setup(p) {

            console.log('Keyword Search setup ...', p)
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
    <div class="row">
        Processes
    </div>
</template>

