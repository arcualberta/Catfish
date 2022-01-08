<script lang="ts">
	import { defineComponent, PropType } from 'vue'
	import { useStore } from 'vuex';

	import { Text } from '../../../models/textModels'

    export default defineComponent({
        name: "TextArea",
        props: {
            model: {
                type: null as PropType<Text> | null,
                required: true
            },
            isRequired:
            {
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
					console.log(value)
					//this.store.commit('SET_TEXT_VALUE', { id: this.model.id, val: value });
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
    });
</script>

<template>
    <div>
        <textarea v-model="content" placeholder="add multiple lines" required="{isRequired ? 'required' : ''}" class="form-control"></textarea>
    </div>
</template>

