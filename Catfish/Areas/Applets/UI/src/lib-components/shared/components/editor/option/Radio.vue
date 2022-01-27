<script lang="ts">
	import { defineComponent, PropType } from 'vue'
	import { useStore } from 'vuex';
	import Editor from '@tinymce/tinymce-vue'

	import { FlattenedFormFiledMutations } from '../../../store/form-submission-utils'
	import { eValidationStatus } from '../../../models/fieldContainer'
	import { Option } from '../../../models/fieldContainer'

    export default defineComponent({
        name: "Text",
        props: {
            model: {
                type: null as PropType<Option> | null,
                required: true
			},
			validationStatus: {
				type: null as PropType<eValidationStatus> | null,
				required: true
			},
			name: {
				type: String,
				required: true
            }
        },
		components: {
			Editor
		},
		computed: {
			content: {
				get(): boolean {
					return this.model.selected;
				},
				set(value: boolean) {
                    //console.log("value to be set: " + value + " id: " + this.model.id);
                    this.store.commit(FlattenedFormFiledMutations.SET_OPTION_VALUE, { id: this.model.id, isSelected: value });
                }
            }
		},
		setup(p) {

			const store = useStore();
			//const field = p.field;
			//console.log("radio options: ", JSON.stringify(p.model.optionText?.values.$values));
			console.log("validationStatus: " + p.validationStatus);
			return {
				store,
				//field
			}
		},
		methods: {
			selectOption(event: any, model: Option) {
                var selectedId = event.target.value;
				console.log("selectedID : ", selectedId);
				

				console.log("option model.id " + model.id + "selectedId :" + selectedId)
				//model.selected = true;
				var isSelected = model.id === selectedId ? true : false;
				this.store.commit(FlattenedFormFiledMutations.SET_OPTION_VALUE, {
					selectedId, isSelected
				});
            }
        }
    });
</script>

<template>
	<div v-for="text in model.optionText.values.$values">
		<!--<input type="radio" :model="model.selected" :value="model.id" :name="name" @change="selectOption($event, model)" :selected="model.selected"/><label>{{text.value}}</label>-->
		<input type="radio" :model="content" :value="model.id" :name="name" :id="model.id" @change="selectOption($event, model)"/><label>{{text.value}}</label>
	</div>
	<!--{{JSON.stringify(model.optionText.values?.$values)}}-->
</template>

