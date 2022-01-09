<script lang="ts">
	import { FlattenedFormFiledMutations } from '../../../store/form-submission-utils'
	import { defineComponent, PropType } from 'vue'
	import { useStore } from 'vuex';
	import { Text } from '../../../models/textModels'

    export default defineComponent({
        name: "Text",
        props: {
            model: {
                type: null as PropType<Text> | null,
                required: true
            },
            isRequired: {
                type: Boolean,
                required: false,
                default: false
            }
        },
		computed: {
			content: {
				get(): string {
					return this.model.value;
				},
				set(value: string) {
					//console.log("id:", this.model.id, "   value: ", value)
					this.store.commit(FlattenedFormFiledMutations.SET_TEXT_VALUE, { id: this.model.id, val: value });
				}
			}
		},
		setup(p) {

			const store = useStore();
			const model = p.model;

			console.log(store.state.form?.id);
			console.log(model.id);

			return {
				store
			}
		},
   //     setup(p) {

   //         const store = useStore();
   //         const model = p.model;

			//console.log(store.state.form?.id);
   //         console.log(model.id);

   //         return {
			//	content: computed(() => store.state.itemTemplateId.toString())
			//}
   //     },
    });
</script>

<template>
    <input v-model="content" required="{isRequired ? 'required' : ''}" class="form-control" />
	<b>You entered:</b>
	<p>{{content}}</p>
</template>

