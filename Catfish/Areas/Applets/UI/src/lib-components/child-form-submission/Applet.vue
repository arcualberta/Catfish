<script lang="ts">
	import { Guid } from 'guid-typescript'
	import { defineComponent, computed } from 'vue';
	import { useStore } from 'vuex';
	import props, { QueryParameter, DataAttribute } from '../shared/props'
	import { eValidationStatus } from '../shared/models/fieldContainer'
	import { state } from './store/state'
	import { actions, Actions } from './store/actions'
	import { getters } from './store/getters'
	import { Mutations, SubmissionStatus as eSubmissionStatus } from '../form-submission/store/mutations'
	import { mutations, Mutations as ChildMutations } from './store/mutations'

	import ChildForm from '../shared/components/editor/FieldContainer.vue'
	import ChildView from '../shared/components/display/FieldContainer.vue'

    export default defineComponent({
        name: "ChildFormSubmission",
		components: {
			ChildForm,
			ChildView
        },
        props,
        setup(p) {
           
           // console.log('props: ', JSON.stringify(p));
			const queryParameters = p.queryParameters as QueryParameter;
			const dataAttributes = p.dataAttributes as DataAttribute;

			const itemId = Guid.parse(queryParameters.iid as string);
			const itemTemplateId = Guid.parse(dataAttributes["template-id"] as string);
			const childFormId = Guid.parse(dataAttributes["child-form-id"] as string);

            const store = useStore();

			store.commit(Mutations.SET_ITEM_TEMPLATE_ID, itemTemplateId);
			store.commit(Mutations.SET_FORM_ID, childFormId);
			store.commit(ChildMutations.SET_PATENT_ITEM_ID, itemId);

			//load the data
			store.dispatch(Actions.LOAD_FORM);
			store.dispatch(Actions.LOAD_SUBMISSIONS);
			//const submissionStatus = store.state.submissionStatus as SubmissionStatus;
           // const submissionStatus: eSubmissionStatus = store.state.submissionStatus as eSubmissionStatus;
			//console.log("initial status " + JSON.stringify(submissionStatus));
			return {
				childForm: computed(() => store.state.form),
                childSubmissions: computed(() => store.state.formInstances?.$values),
				store,
				submissionStatus: computed(() => store.state.submissionStatus),
				eSubmissionStatus,
				eValidationStatus
              
            }
		},
		storeConfig: {
			state,
			actions,
			mutations,
			getters
        },
        methods: {
			submitChildForm() {
				this.store.dispatch(Actions.SUBMIT_CHILD_FORM);
            }
        }
    });
</script>

<template>
	<div v-if="childForm && Object.keys(childForm).length > 0">
		<ChildForm :model="childForm" />

		<div v-if="childForm?.validationStatus === eValidationStatus.INVALID" class="alert alert-danger">Form validation failed.</div>
		<div v-else>
			<div v-if="submissionStatus === eSubmissionStatus.InProgress" class="alert alert-info">Submitting...</div>
			<div v-if="submissionStatus === eSubmissionStatus.Success" class="alert alert-info">Submission successful</div>
			<div v-if="submissionStatus === eSubmissionStatus.Fail" class="alert alert-danger">Submission failed</div>
		</div>
		<button class="btn btn-primary" @click="submitChildForm()">Submit</button>
	</div>
	<div v-if="childSubmissions && childSubmissions.length > 0">
		<h3>Responses</h3>
		<div v-for="child in childSubmissions">
			<ChildView :model="child" />
			<hr />
		</div>
	</div>
</template>
