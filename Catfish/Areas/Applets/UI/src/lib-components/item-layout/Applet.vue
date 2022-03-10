<script lang="ts">
	// import { Guid } from 'guid-typescript'
	import { defineComponent, computed } from 'vue'
	import { useStore } from 'vuex';

	import { state } from './store/state'
	import { actions/*, Actions */ } from './store/actions'
	import { Actions as ItemAction } from '../item-viewer/store/actions'
	import { getters } from './store/getters'
	import { mutations,  } from './store/mutations'
	import { Mutations } from '../item-viewer/store/mutations';

	import props, { QueryParameter, DataAttribute } from '../shared/props'
	//import { FieldLayout } from "./models/fieldLayout"
	import FieldComponent from "./components/fieldComponent.vue"


	export default defineComponent({
		name: "ItemLayout",
		components: {
			FieldComponent
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

			//get all the unique formIds
			//let uniqueFormIds = [...new Set(components.map((com: FieldLayout) => com.formTemplateId))];
			//  console.log("selected Forms Ids" + JSON.stringify(uniqueFormIds));


			//store.commit(Mutations.SET_FORM_IDS, uniqueFormIds);

			//load the data
			store.dispatch(ItemAction.LOAD_ITEM);
			// console.log("selected Forms" + JSON.stringify(store.state.item));

			//get all the selected Fields
			//const fields = store.getters.fields(components);

			return {
				store,
				item: computed(() => store.state.item),
				queryParams,
				//dataItem: computed(() => store.getters.rootDataItem),
				isAdmin,
				components,
				//items: computed(() => store.state.items)
				fields: computed(() => store.getters.fields(components))
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
		<h3>ItemLayout</h3>

		<h4>Compunents</h4>
		<!--{{JSON.stringify(components)}}-->

		<FieldComponent v-for="field in fields" :model="field" />
	</div>
</template>

