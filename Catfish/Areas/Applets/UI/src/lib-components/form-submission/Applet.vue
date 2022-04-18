<script lang="ts">
	import { Guid } from 'guid-typescript'
	import { defineComponent, computed } from 'vue';
	import { useStore } from 'vuex';
	import props, { DataAttribute } from '../shared/props'
	import { eValidationStatus } from '../shared/models/fieldContainer'
	import { state, State } from './store/state'
	import { actions, Actions } from './store/actions'
    import { mutations, Mutations } from './store/mutations'
    import { eSubmissionStatus, FlattenedFormFiledMutations } from '../shared/store/form-submission-utils'
	

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
			const collectionId = Guid.parse(dataAttributes["collection-id"] as string);
			const groupId = dataAttributes["group-id"] ? Guid.parse(dataAttributes["group-id"] as string) : null;

            const store = useStore();

			store.commit(FlattenedFormFiledMutations.REMOVE_FIELD_CONTAINERS);
			store.commit(Mutations.SET_ITEM_TEMPLATE_ID, itemTemplateId);
			store.commit(Mutations.SET_FORM_ID, formId);
			store.commit(Mutations.SET_COLLECTION_ID, collectionId);
			store.commit(Mutations.SET_GROUP_ID, groupId);
			//load the data
			store.dispatch(Actions.LOAD_FORM);

			return {
				store,
				submissionForm: computed(() => (store.state as State).form),
				submissionStatus: computed(() => (store.state as State).submissionStatus),
				eSubmissionStatus,
				eValidationStatus,
                submitForm: () => store.dispatch(Actions.SUBMIT_FORM)
              
            }
		},
		storeConfig: {
			state,
			actions,
			mutations
        },
    });
</script>

<template>
	<div v-if="submissionForm && Object.keys(submissionForm).length > 0">
		<SubmissionForm :model="submissionForm" />
		<button class="btn btn-primary" @click="submitForm()">Submit</button>
	</div>
	<div v-if="submissionForm?.validationStatus === eValidationStatus.INVALID" class="alert alert-danger">Form validation failed.</div>
	<div v-else>
		<div v-if="submissionStatus === eSubmissionStatus.InProgress" class="alert alert-info">Submitting...</div>
		<div v-if="submissionStatus === eSubmissionStatus.Success" class="alert alert-info">Submission successful</div>
		<div v-if="submissionStatus === eSubmissionStatus.Fail" class="alert alert-danger">Submission failed</div>
	</div>
</template>
