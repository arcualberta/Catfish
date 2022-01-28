<script lang="ts">
	import { defineComponent, PropType } from 'vue'
	import { useStore } from 'vuex';

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
		computed: {
			selected: {
				get(): boolean {
					return this.model.selected;
				},
				set(value: boolean) {
                    console.log("selected value: ", value);
                    //this.store.commit(FlattenedFormFiledMutations.SET_OPTION_VALUE, { id: this.model.id, isSelected: value });
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
		<input type="radio" :name="name" :id="model.id" :value="model.id" v-model="selected" /> <labe :for="model.id">{{text.value}}</labe>
	</div>
	<!--{{JSON.stringify(model.optionText.values?.$values)}}-->
</template>

