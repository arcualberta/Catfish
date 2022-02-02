<script lang="ts">
	import { Guid } from 'guid-typescript'
	import { defineComponent, computed } from 'vue';
	import { useStore } from 'vuex';
	import props, { DataAttribute } from '../shared/props'
	import { eValidationStatus } from '../shared/models/fieldContainer'
	import { state } from './store/state'
	import { actions, Actions } from './store/actions'
	import { getters } from './store/getters'
	import { mutations, Mutations } from './store/mutations'
	import { eSubmissionStatus } from '../shared/store/form-submission-utils'
	

	import SubmissionForm from '../shared/components/editor/FieldContainer.vue'

    export default defineComponent({
        name: "FormSubmission",
		components: {
			SubmissionForm
        },
        props,
        setup(p) {
           
            console.log('props: ', JSON.stringify(p));
			const dataAttributes = p.dataAttributes as DataAttribute;

			const itemTemplateId = Guid.parse(dataAttributes["template-id"] as string);
			const formId = Guid.parse(dataAttributes["form-id"] as string);

            const store = useStore();

			store.commit(Mutations.SET_ITEM_TEMPLATE_ID, itemTemplateId);
			store.commit(Mutations.SET_FORM_ID, formId);

			//load the data
			store.dispatch(Actions.LOAD_FORM);

			return {
				store,
				submissionForm: computed(() => store.state.form),
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
			submitForm() {
				this.store.dispatch(Actions.SUBMIT_FORM);
            }
        }
    });
</script>

<template>
	<div v-if="submissionForm && Object.keys(submissionForm).length > 0">
		<SubmissionForm :model="submissionForm" />

		<div v-if="submissionForm?.validationStatus === eValidationStatus.INVALID" class="alert alert-danger">Form validation failed.</div>
		<div v-else>
			<div v-if="submissionStatus === eSubmissionStatus.InProgress" class="alert alert-info">Submitting...</div>
			<div v-if="submissionStatus === eSubmissionStatus.Success" class="alert alert-info">Submission successful</div>
			<div v-if="submissionStatus === eSubmissionStatus.Fail" class="alert alert-danger">Submission failed</div>
		</div>
		<button class="btn btn-primary" @click="submitForm()">Submit</button>
	</div>
</template>
