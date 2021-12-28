<script lang="ts">
	import { defineComponent, onMounted, computed } from 'vue';
	import { useStore } from 'vuex'

    import props from '../shared/props'
	import { Actions, actions } from '../shared/store/actions'
	import { Mutations, mutations } from '../shared/store/mutations'
    import { getters } from '../shared/store/getters'
    import { Grid } from '../shared/models/cmsModels'
	import CardTemplate from './components/CardTemplate.vue'

    export default defineComponent({
		name: "Applet",
        components: {
			CardTemplate
        },
        props,
        setup(p) {
            console.log('Grid setup ...')

			const store = useStore()

			//Storing the page and block IDs in the store
			store.commit(Mutations.SET_SOURCE, { pageId: p.pageId, blockId: p.blockId });

            //When the component is mounted, load the grid contents.
            onMounted(() => store.dispatch(Actions.LOAD_BLOCK));

            return {
				model: computed(() => store.state.model as Grid)
			}
        },
        storeConfig: {
			actions,
			mutations,
			getters
		}
    });
</script>

<template>
    <div>
        <h2>Grid</h2>
        <div class="row">
            <CardTemplate v-for="card in model?.items" :model="card" />
        </div>
        <!--<div class="row">{{JSON.stringify(model)}}</div>-->
    </div>
</template>
