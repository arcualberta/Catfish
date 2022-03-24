<script lang="ts">
	import { Guid } from 'guid-typescript'

	import { defineComponent, computed } from 'vue';
	import { useStore } from 'vuex';
    import props, { DataAttribute, QueryParameter } from '../shared/props'

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

            const queryParams = p.queryParameters as QueryParameter;
            store.commit(Mutations.SET_ID, queryParams.iid);

            const detailedViewURL = dataAttributes["detailed-url"] as string;
            store.commit(Mutations.SET_DETAILED_VIEW_URL, detailedViewURL)
            store.state.detailedViewUrl = detailedViewURL;
		

            console.log("detailed-view-url " + detailedViewURL)
            //console.log("selected Fields: " + selectedFields)
			

			


			return {
				store,
				selectedFields,
                reportRows: computed(() => store.state.reportData),
                loadData: () => store.dispatch(Actions.LOAD_DATA),
				queryParams,
                detailedViewURL
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
				<th></th>
				<th v-for="field in selectedFields">{{field.fieldName}}</th>
			</tr>
		</thead>
		<tbody>
			<tr v-for="reportRow in reportRows">
				<td>
					<button class="item-list-image" @click="$router.push({name: '{{detailedViewURL}}', params: { id: '{{reportRow.itemId'}} }})"></button>
					<!--<div class="item-list-image">{{reportRow.itemId}}</div>-->
				</td>
				<td v-for="cell in reportRow.cells">
					<div v-for="cellValue in cell.values">
						<div v-if="cellValue.renderType === 'MultilingualText'">
							<div v-for="txt in cellValue.values">
								{{txt.value}}
							</div>
						</div>
						<div v-if="cellValue.renderType === 'Options'">
							<div v-for="txt in cellValue.values">
								{{txt.value}}
							</div>
						</div>
						<div v-if="cellValue.renderType === 'MonolingualText'">
							<div v-for="txt in cellValue.values">
								{{txt.value}}
							</div>
						</div>
						<div v-if="cellValue.renderType === 'Attachment'">
							<div v-for="txt in cellValue.values">
								{{txt.value}}
							</div>
						</div>
						<div v-if="cellValue.renderType === 'Audio'">
							<div v-for="txt in cellValue.values">
								{{txt.value}}
							</div>
						</div>
					</div>
				</td>

			</tr>
		</tbody>
	</table>

	<!--<div v-for="reportRow in reportRows">
		<h3>Row</h3>
		<div v-for="cell in reportRow.cells.$values">
			<h4>Cell</h4>
			<div v-for="cellValue in cell.values.$values">
				<h5>Cell Value</h5>
				{{cellValue.$type}}
				{{cellValue.formInstanceId}}
			</div>
		</div>

	</div>-->
</template>

