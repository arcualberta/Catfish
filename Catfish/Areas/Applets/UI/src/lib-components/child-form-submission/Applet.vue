<script lang="ts">
	import { Guid } from 'guid-typescript'

	//import { defineComponent, computed } from 'vue';
	import { defineComponent, computed, ref } from 'vue';
	import { useStore } from 'vuex';
	import props, { QueryParameter, DataAttribute } from '../shared/props'
	import { eValidationStatus, FieldContainer } from '../shared/models/fieldContainer'
	import { state } from './store/state'
	import { actions, Actions } from './store/actions'
	import { getters } from './store/getters'
	import { Mutations } from '../form-submission/store/mutations'
	import { mutations, Mutations as ChildMutations } from './store/mutations'
	import { eSubmissionStatus } from '../shared/store/form-submission-utils'

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
			const childResponseFormIdStr = dataAttributes["response-form-id"] as string;
			const isAdmin = dataAttributes["is-admin"] as string;
			const childResponseFormId = childResponseFormIdStr?.length > 0 ? Guid.parse(childResponseFormIdStr) : undefined;

			const store = useStore();

			store.commit(Mutations.CLEAR_FLATTENED_FIELD_MODELS);
			store.commit(Mutations.SET_ITEM_TEMPLATE_ID, itemTemplateId);
			store.commit(Mutations.SET_FORM_ID, childFormId);
			store.commit(ChildMutations.SET_PARENT_ITEM_ID, itemId);

			//load the data
			store.dispatch(Actions.LOAD_FORM);
			store.dispatch(Actions.LOAD_SUBMISSIONS);
			if (childResponseFormId) {
				store.commit(ChildMutations.SET_RESPONSE_FORM_ID, childResponseFormId);
				store.dispatch(Actions.LOAD_RESPONSE_FORM);
			}

			//const submissionStatus = store.state.submissionStatus as SubmissionStatus;
			//const submissionStatus: eSubmissionStatus = store.state.submissionStatus as eSubmissionStatus;
			//console.log("initial status " + JSON.stringify(submissionStatus));

			const responseDisplayFlags = ref([] as boolean[]);
			const childSubmissions = computed(() => store.state.formInstances?.$values);

			const toggleDisplayResponse = (index: number) => {
				if (responseDisplayFlags.value[index] != undefined) {
					responseDisplayFlags.value[index] = !responseDisplayFlags.value[index]
				}
				else {
					responseDisplayFlags.value[index] = !responseDisplayFlags.value[index] //true;
				}

				//Closing all other response boxes
				responseDisplayFlags.value.forEach((val, idx) => {
					if (val && idx !== index)
						responseDisplayFlags.value[idx] = false;
				})

				return false;
			}

			const submitChildResponse = (index: number) => {
				store.dispatch(Actions.SUBMIT_CHILD_RESPONSE_FORM, (childSubmissions.value[index] as FieldContainer)?.id)
				toggleDisplayResponse(index);
			}

			return {
				childForm: computed(() => store.state.form),
				childSubmissions: computed(() => store.state.formInstances?.$values),
				store,
				submissionStatus: computed(() => store.state.submissionStatus),
				eSubmissionStatus,
				eValidationStatus,
				childResponseFormId,
				childResponseForm: computed(() => store.state.childResponseForm),
				responseDisplayFlags,
				toggleDisplayResponse,
				submitChildResponse,
				isAdmin
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
			},
			
			removeResponseForm(itemToRemove: FieldContainer) {
		
				if (confirm("Do you really want to delete this item?")) {
                    this.store.dispatch(Actions.DELETE_CHILD_RESPONSE_INSTANCE, itemToRemove);
				}
			},
            removeChildForm(itemToRemove: FieldContainer) {

                if (confirm("Do you really want to delete this item?")) {
                    this.store.dispatch(Actions.DELETE_CHILD_INSTANCE, itemToRemove);
                }
            }
		}
	});
</script>

<template  class="childFormSubmissionApplet">
	<div v-if="childForm && Object.keys(childForm).length > 0" class="submissionForm">
		<ChildForm :model="childForm" />
		<div v-if="childForm?.validationStatus === eValidationStatus.INVALID" class="alert alert-danger">Form validation failed.</div>
		<div v-else>
			<div v-if="submissionStatus === eSubmissionStatus.InProgress" class="alert alert-info">Submitting...</div>
			<div v-if="submissionStatus === eSubmissionStatus.Success" class="alert alert-info">Submission successful</div>
			<div v-if="submissionStatus === eSubmissionStatus.Fail" class="alert alert-danger">Submission failed</div>
		</div>
		<button class="btn btn-primary" @click="submitChildForm()">Submit</button>
	</div>
	<div v-if="childSubmissions && childSubmissions.length > 0" class="mt-2 submissionInstanceList">
		<h3>Responses</h3>
		<div v-for="(child, index) in childSubmissions" class="submissionInstance">
			
			<ChildView :model="child" :hide-field-names="true"  />
			<div class="text-right" v-if="!responseDisplayFlags[index]">
				<a href="#" class="text-decoration-none" @click="toggleDisplayResponse(index)" onclick="return false;"><span class="fas fa-reply replyBtn"></span></a>
				<span v-if="isAdmin" class="fas fa-remove" @click="removeChildForm(child);"></span>
			</div>
			<!--{{JSON.stringify(child)}}-->
			<div class="ml-3 submissionInstanceList">
				<div v-if="childResponseFormId" class="mb-2">
					<div v-if="responseDisplayFlags[index]" class="childResponseForm">
						<ChildForm :model="childResponseForm" />
						<div v-if="childResponseForm?.validationStatus === eValidationStatus.INVALID" class="alert alert-danger">Response validation failed.</div>
						<button class="btn btn-primary submitBtn" @click="submitChildResponse(index)">Submit</button>
					</div>
				</div>
				<div v-for="(response, resIdx) in child.childFieldContainers.$values" class="submissionInstance">
					<ChildView :model="response" :hide-field-names="true" />


					<div class="text-right" v-if="isAdmin"><span class="fas fa-remove deleteBtn" @click="removeResponseForm(response);"></span></div>
				</div>
			</div>
		</div>
	</div>
</template>

<style scoped>
	.fa-remove {
		color: red;
		margin-left: 30px;
	}
</style>
