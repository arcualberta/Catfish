<script lang="ts">
	import { defineComponent, computed } from 'vue'
	import { useStore } from 'vuex';

	import { state } from './store/state'
	import { actions/*, Actions */ } from './store/actions'
	import { Actions as ItemAction } from '../item-viewer/store/actions'
	import { getters } from './store/getters'
	import { mutations,  } from './store/mutations'
	import { Mutations } from '../item-viewer/store/mutations';

	import props, { QueryParameter, DataAttribute } from '../shared/props'

	export default defineComponent({
		name: "ItemLayout",
		components: {
		},
		props,
		setup(p) {
			const store = useStore();

			const queryParams = p.queryParameters as QueryParameter;
			store.commit(Mutations.SET_ID, queryParams.iid);

			const dataAttributes = p.dataAttributes as DataAttribute;

			//const templateId = dataAttributes["template-id"] as string;
			//store.commit(Mutations.SET_TEMPLATE_ID, templateId);

			const selectedComponents = dataAttributes["selected-components"] as string;
			const components = JSON.parse(selectedComponents);

			const isAdmin = dataAttributes["is-admin"] as string;

		

			//load the data
			store.dispatch(ItemAction.LOAD_ITEM);
			// console.log("selected Forms" + JSON.stringify(store.state.item));
            console.log("components length:" + components.length);
			//const fields = store.getters.fields(components);

           // console.log("fields length:" + fields.length);

            const staticFields = components.filter((comp:any) => comp.$type.includes("StaticText"));
            console.log("static Field:" + staticFields.length);
			console.log(JSON.stringify(staticFields));
			return {
				store,
				item: computed(() => store.state.item),
				queryParams,	
				isAdmin,
				components,
                staticFields,
				fields: computed(() => store.getters.fields(components)),
              
			}
		},
		storeConfig: {
			state,
			actions,
			mutations,
			getters,

		}
	});
</script>

<template>
	<div class="item">
	</div>
</template>

