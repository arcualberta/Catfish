<script lang="ts">
	import { defineComponent, PropType } from 'vue'
	import { useStore } from 'vuex';
	import Editor from '@tinymce/tinymce-vue'

	import { FlattenedFormFiledMutations } from '../../../store/form-submission-utils'
	import { eValidationStatus } from '../../../models/fieldContainer'
	import { Text } from '../../../models/textModels'

    export default defineComponent({
        name: "Text",
        props: {
            model: {
                type: null as PropType<Text> | null,
                required: true
			},
			isMultiline: {
				type: Boolean,
				required: true
			},
			isRichText: {
				type: Boolean,
				required: true
			},
			validationStatus: {
				type: null as PropType<eValidationStatus> | null,
				required: true
			},
			field: {
				type: String,
				required: false,
				default: "text"
            }
        },
		components: {
			Editor
		},
		computed: {
			content: {
				get(): string {
					return this.model.value;
				},
				set(value: string) {
					//console.log("value to be set: " + value + " id: " + this.model.id);
					this.store.commit(FlattenedFormFiledMutations.SET_TEXT_VALUE, { id: this.model.id, val: value });
				}
			}
		},
		setup(p) {

			const store = useStore();
			const field = p.field;
            //console.log("validationStatus: " + p.validationStatus)

			return {
				store,
				field
			}
		},
    });
</script>

<template>
	<Editor v-if="isRichText" apiKey="0ohehg73era56wydy5kyws6ouf25550ogy2sifi1j41hk65l" v-model="content" placeholder="add multiple lines" />
	<textarea v-else-if="isMultiline" v-model="content" />
	<div v-else>
		<input v-if="field === 'text'" type="text" v-model="content" />
		<input v-else-if="field === 'decimal'" type="number" v-model="content" step="0.01" placeholder="0.00" />
		<input v-else :type="field" v-model="content" />

	</div>
</template>

