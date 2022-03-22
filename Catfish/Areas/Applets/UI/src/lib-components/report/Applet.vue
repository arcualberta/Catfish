<script lang="ts">
	import { Guid } from 'guid-typescript'

	import { defineComponent, computed } from 'vue';
	import { useStore } from 'vuex';
	import props, {  DataAttribute } from '../shared/props'

	import { state } from './store/state'
	import { actions, Actions } from './store/actions'
	import { getters } from './store/getters'
	import { mutations, Mutations } from './store/mutations'

	export default defineComponent({
		name: "Report",
		components: {
			
		},
		props,
		setup(p) {

			const store = useStore();

			// console.log('props: ', JSON.stringify(p));
			//const queryParameters = p.queryParameters as QueryParameter;
			const dataAttributes = p.dataAttributes as DataAttribute;


			const itemTemplateId = Guid.parse(dataAttributes["template-id"] as string);
			store.commit(Mutations.SET_TEMPLATE_ID, itemTemplateId)
			store.state.itemTemplateId = itemTemplateId;

			const collectionId = Guid.parse(dataAttributes["collection-id"] as string);
            store.commit(Mutations.SET_COLLECTION_ID, collectionId)
			store.state.collectionId = collectionId;

            const groupId = Guid.parse(dataAttributes["group-id"] as string);
            store.commit(Mutations.SET_GROUP_ID, groupId)
            store.state.groupId = groupId;

			const selectedFields = JSON.parse(dataAttributes["selected-fields"] as string);
            store.commit(Mutations.SET_REPORT_FIELDS, selectedFields)
            store.state.reportFields = selectedFields;
            //console.log("item template id " + itemTemplateId)
            //console.log("selected Fields: " + selectedFields)
			const isAdmin = dataAttributes["is-admin"] as string;
			

			

			

			//store.dispatch(Actions.LOAD_SUBMISSIONS);
			//if (childResponseFormId) {
			//	store.commit(ChildMutations.SET_RESPONSE_FORM_ID, childResponseFormId);
			//	store.dispatch(Actions.LOAD_RESPONSE_FORM);
			//}

			//const submissionStatus = store.state.submissionStatus as SubmissionStatus;
			//const submissionStatus: eSubmissionStatus = store.state.submissionStatus as eSubmissionStatus;
			//console.log("initial status " + JSON.stringify(submissionStatus));

			

			return {
				store,
				selectedFields,
				reportRows: computed(() => state.reportData),
				isAdmin,
				loadData: () => store.dispatch(Actions.LOAD_DATA)
			}
		},
		storeConfig: {
			state,
			actions,
			mutations,
			getters
		},
		//methods: {
  //          LoadData() {
  //              this.store.dispatch(Actions.LOAD_DATA);
  //          }
  //      }
			
	});
</script>

<template class="report">
	<h3>Report</h3>
	<button class="btn btn-primary" @click="loadData()">Execute</button>
	<div>{{selectedFields}}</div>
	<div>{{selectedFields.length}}</div>

	<table class="table">
		<thead>
			<tr>
				<th v-for="field in selectedFields">{{field.fieldName}}</th>
			</tr>
		</thead>
		<tbody>
			<tr v-for="reportRow in reportRows">
				<td v-for="cell in reportRow.cells">
					{{cell.value}}
				</td>

			</tr>
		</tbody>
	</table>
	<div>{{reportRows}}</div>
</template>

