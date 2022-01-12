<script lang="ts">
	import { Guid } from 'guid-typescript'
	import { defineComponent, computed } from 'vue';
	import { useStore } from 'vuex';
	import props, { QueryParameter, DataAttribute } from '../shared/props'
	import { state } from './store/state'
	import { actions, Actions } from './store/actions'
	import { getters } from './store/getters'
	import { mutations, Mutations } from './store/mutations'

	import FieldContainer from '../shared/components/editor/FieldContainer.vue'

    export default defineComponent({
        name: "ChildFormSubmission",
		components: {
			FieldContainer
        },
        props,
        setup(p) {
           
            console.log('props: ', JSON.stringify(p));
			const queryParameters = p.queryParameters as QueryParameter;
			const dataAttributes = p.dataAttributes as DataAttribute;

			const itemId = Guid.parse(queryParameters.iid as string);
			const itemTemplateId = Guid.parse(dataAttributes["template-id"] as string);
			const childFormId = Guid.parse(dataAttributes["child-form-id"] as string);

			const store = useStore();
			store.commit(Mutations.SET_IDS, [itemId, itemTemplateId, childFormId]);

			//load the data
			store.dispatch(Actions.LOAD_FORM);
			store.dispatch(Actions.LOAD_SUBMISSIONS);


			return {
				childForm: computed(() => store.state.form),
				childSubmissions: computed(() => store.state.formInstances)
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
	<div>
		<FieldContainer :model="childForm" v-if="childForm" />
		<!--<div>{{JSON.stringify(childForm)}}</div>-->

		<h3>Child Submissions</h3>
		<div>{{JSON.stringify(childSubmissions)}}</div>
	</div>
</template>
