<script lang="ts">
	//import { Guid } from 'guid-typescript'

	//import { defineComponent, computed } from 'vue';
	import { defineComponent } from 'vue';
	import { useStore } from 'vuex';
	import props, {  DataAttribute } from '../shared/props'
	

	
	export default defineComponent({
		name: "Report",
		components: {
			
		},
		props,
		setup(p) {

			// console.log('props: ', JSON.stringify(p));
			//const queryParameters = p.queryParameters as QueryParameter;
			const dataAttributes = p.dataAttributes as DataAttribute;

			
			//const itemTemplateId = Guid.parse(dataAttributes["template-id"] as string);
			const selectedFields = dataAttributes["selected-fields"] as string;
            //console.log("item template id " + itemTemplateId)
            //console.log("selected Fields: " + selectedFields)
			const isAdmin = dataAttributes["is-admin"] as string;
			
			const store = useStore();

			

			//load the data
			//store.dispatch(Actions.LOAD_FORM);
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
				isAdmin
			}
		},
		storeConfig: {
			//state,
			//actions,
			//mutations,
			//getters
		}
		
			
	});
</script>

<template  class="report">
	<h3>Report</h3>
	<div>{{selectedFields}}</div>
</template>

