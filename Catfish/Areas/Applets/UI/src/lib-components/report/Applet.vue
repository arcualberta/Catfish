<script lang="ts">
	import { Guid } from 'guid-typescript'

	import { defineComponent, computed, ref } from 'vue';
	import { useStore } from 'vuex';
    import props, { DataAttribute, QueryParameter } from '../shared/props'

	import { SearchParams, state, SystemStatus } from './store/state'
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

			const templateStatus = JSON.parse(dataAttributes["status"] as string) as SystemStatus[];
            store.commit(Mutations.SET_STATUS, templateStatus)
            store.state.templateStatus = templateStatus;
			
            const queryParams = p.queryParameters as QueryParameter;
            store.commit(Mutations.SET_ID, queryParams.iid);

            const detailedViewUrlPath = dataAttributes["detailed-url"] as string;
            //store.commit(Mutations.SET_DETAILED_VIEW_URL, detailedViewURL)
            //store.state.detailedViewUrl = detailedViewURL;
		

            //console.log("detailed-view-url " + detailedViewURL)
            //console.log("selected Fields: " + selectedFields)
			

            const fromDate = ref(null);
            const toDate = ref(null);
            const selectedStatus = ref(null);


			return {
				store,
				selectedFields,
                reportRows: computed(() => store.state.reportData),
                loadData: () => store.dispatch(Actions.LOAD_DATA, { startDate: fromDate.value, endDate: toDate.value, status: selectedStatus.value } as SearchParams),
				queryParams,
                templateStatus,
				detailedViewURL: (id: Guid) => { const url = detailedViewUrlPath + id; return url; },
				fromDate,
				toDate,
                selectedStatus
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
	<div class="row col-md-12">
		<div class="col-md-6 form-group">
			<label>From:</label>
			<!--<input type="date" name="startDate" id="startDate" v-model="startDate" class="form-control" />-->
			<input type="date" name="startDate" id="startDate" v-model="fromDate" class="form-control" />
		</div>
		<div class="col-md-6 form-group">
			<label class="form-label">To:</label>
			<input type="date" name="endDate" id="endDate" v-model="toDate" class="form-control" />
		</div>
		<div class="col-md-6 form-group">
			<label class="form-label">Status:</label>
			<select v-model="selectedStatus" class="form-control" style="width:auto;">
				<option disabled value="">Please select one</option>
				<option v-for="status in templateStatus" :value="status.id">{{status.status}}</option>
			</select>
		</div>
		<div class="col-md-6 form-group">
			<button class="btn btn-primary" @click="loadData()">Execute</button>
		</div>
			<!--<button onclick="filterItems('@entityTemplateId', '@collectionId', $('#startDate').val(), $('#endDate').val(), 'itemListBlockTable',@reportTemplateId);" class="btn btn-default btn-primary" style="margin-top:30px; height:fit-content;" value="Execute">Go<i class="fa fa-arrow-right" style="padding-left:5px;"></i></button>-->
		</div>
		<table class="table">
			<thead>
				<tr>
					<th></th>
					<th v-for="field in selectedFields">{{field.fieldName}}</th>
					<th>Submitted Date</th>
					<th>Status</th>
				</tr>
			</thead>
			<tbody>
				<tr v-for="reportRow in reportRows">
					<td>
						<a :href="detailedViewURL(reportRow.itemId)" class="fa fa-eye" target="_blank"></a>
					</td>
					<td v-for="cell in reportRow.cells">
						<div v-for="cellValue in cell.values">
							<div v-if="cellValue.renderType === 'MultilingualText'">
								<div v-for="txt in cellValue.values">
									{{txt.value}}
								</div>
							</div>
							<ul v-if="cellValue.renderType === 'Options'">
								<li v-for="txt in cellValue.values">
									{{txt.value}}
								</li>
							</ul>
							<div v-if="cellValue.renderType === 'MonolingualText'">
								<div v-for="txt in cellValue.values">
									<!--<div v-if="txt"></div>-->
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
					<td>
						<div>{{reportRow.created}}</div>
					</td>
					<td>
						<div>{{reportRow.status}}</div>
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

