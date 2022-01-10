<script lang="ts">
	import { defineComponent, PropType } from 'vue'
	import { useStore } from 'vuex';
	import Editor from '@tinymce/tinymce-vue'

	import { FlattenedFormFiledMutations } from '../../../store/form-submission-utils'
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
            isRequired: {
                type: Boolean,
				required: true
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
					this.store.commit(FlattenedFormFiledMutations.SET_TEXT_VALUE, { id: this.model.id, val: value });
				}
			}
		},
		setup() {

			const store = useStore();

			return {
				store
			}
		},
    });
</script>

<template>
	<Editor v-if="isRichText" apiKey="0ohehg73era56wydy5kyws6ouf25550ogy2sifi1j41hk65l" v-model="content" placeholder="add multiple lines" required="{isRequired ? 'required' : ''}" />
	<textarea v-else-if="isMultiline" v-model="content" required="{isRequired ? 'required' : ''}" />
	<input v-else v-model="content" required="{isRequired ? 'required' : ''}" class="form-control" />
	<div>isRequired: {{isRequired}}</div>
	<div><b>You entered:</b></div>
	<div v-html="content" />
</template>

