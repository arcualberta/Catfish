<script lang="ts">
    import { defineComponent, PropType } from 'vue'
    import { TextCollection } from '../../../models/textModels'
    import Text from './Text.vue'

    export default defineComponent({
        name: "TextCollection",
        props: {
            model: {
                type: null as PropType<TextCollection> | null,
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
            Text
        }
    });
</script>

<template>
    <Text v-for="val in model.values" :model="val" :is-multiline="isMultiline" :is-rich-text="isRichText" :is-required="isRequired" />
</template>

